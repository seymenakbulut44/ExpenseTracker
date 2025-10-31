using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Data;
using ExpenseTracker.Models;

namespace ExpenseTracker.Controllers;

[Authorize]
public class TransactionsController : Controller
{
	private readonly ApplicationDbContext _dbContext;

	public TransactionsController(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<IActionResult> Index()
	{
		var userId = GetUserId();
		var transactions = await _dbContext.Transactions
			.Include(t => t.Category)
			.Where(t => t.UserId == userId)
			.OrderByDescending(t => t.Date)
			.ToListAsync();
		return View(transactions);
	}

	public async Task<IActionResult> Create()
	{
		var userId = GetUserId();
		var categories = await _dbContext.Categories
			.Where(c => c.UserId == userId)
			.OrderBy(c => c.Name)
			.ToListAsync();
		if (!categories.Any())
		{
			// kullanıcıya yönlendirici bilgi mesajı
			TempData["Info"] = "Please create a category before adding transactions.";
			return RedirectToAction("Index", "Categories");
		}
		ViewBag.CategoryId = new SelectList(categories, nameof(Category.Id), nameof(Category.Name));
		return View(new Transaction { Date = DateTime.Today });
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(Transaction transaction)
	{
		if (!ModelState.IsValid)
		{
			await PopulateCategories();
			return View(transaction);
		}
		transaction.UserId = GetUserId();
		_dbContext.Transactions.Add(transaction);
		await _dbContext.SaveChangesAsync();
		return RedirectToAction(nameof(Index));
	}

	public async Task<IActionResult> Edit(int id)
	{
		var userId = GetUserId();
		var transaction = await FindTransactionForUserAsync(id, userId);
		if (transaction == null)
		{
			return NotFound();
		}
		await PopulateCategories();
		return View(transaction);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(int id, Transaction transaction)
	{
		if (id != transaction.Id)
		{
			return BadRequest();
		}
		if (!ModelState.IsValid)
		{
			await PopulateCategories();
			return View(transaction);
		}
		var userId = GetUserId();
		var existing = await FindTransactionForUserAsync(id, userId);
		if (existing == null)
		{
			return NotFound();
		}
		existing.Amount = transaction.Amount;
		existing.Date = transaction.Date;
		existing.Notes = transaction.Notes;
		existing.Type = transaction.Type;
		existing.CategoryId = transaction.CategoryId;
		await _dbContext.SaveChangesAsync();
		return RedirectToAction(nameof(Index));
	}

	public async Task<IActionResult> Delete(int id)
	{
		var userId = GetUserId();
		var transaction = await _dbContext.Transactions.Include(t => t.Category).FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
		if (transaction == null)
		{
			return NotFound();
		}
		return View(transaction);
	}

	[HttpPost, ActionName("Delete")]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> DeleteConfirmed(int id)
	{
		var userId = GetUserId();
		var transaction = await FindTransactionForUserAsync(id, userId);
		if (transaction == null)
		{
			return NotFound();
		}
		_dbContext.Transactions.Remove(transaction);
		await _dbContext.SaveChangesAsync();
		return RedirectToAction(nameof(Index));
	}

	// kullanıcının kimliğini güvenilir şekilde elde eder
	private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

	// işlem sadece kullanıcıya aitse bulunur
	private Task<Transaction?> FindTransactionForUserAsync(int id, string userId)
	{
		return _dbContext.Transactions.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
	}

	private async Task PopulateCategories()
	{
		var userId = GetUserId();
		var categories = await _dbContext.Categories
			.Where(c => c.UserId == userId)
			.OrderBy(c => c.Name)
			.ToListAsync();
		ViewBag.CategoryId = new SelectList(categories, nameof(Category.Id), nameof(Category.Name));
	}
}


