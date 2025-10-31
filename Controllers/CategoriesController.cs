using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Data;
using ExpenseTracker.Models;

namespace ExpenseTracker.Controllers;

[Authorize]
public class CategoriesController : Controller
{
	private readonly ApplicationDbContext _dbContext;

	public CategoriesController(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<IActionResult> Index()
	{
		var userId = GetUserId();
		var categories = await _dbContext.Categories
			.Where(c => c.UserId == userId)
			.OrderBy(c => c.Name)
			.ToListAsync();
		return View(categories);
	}

	public IActionResult Create()
	{
		// boş modelle formu açıyoruz
		return View(new Category());
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(Category category)
	{
		if (!ModelState.IsValid)
		{
			return View(category);
		}
		category.UserId = GetUserId();
		_dbContext.Categories.Add(category);
		await _dbContext.SaveChangesAsync();
		return RedirectToAction(nameof(Index));
	}

	public async Task<IActionResult> Edit(int id)
	{
		var userId = GetUserId();
		var category = await FindCategoryForUserAsync(id, userId);
		if (category == null)
		{
			return NotFound();
		}
		return View(category);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(int id, Category category)
	{
		if (id != category.Id)
		{
			return BadRequest();
		}
		if (!ModelState.IsValid)
		{
			return View(category);
		}
		var userId = GetUserId();
		var existing = await FindCategoryForUserAsync(id, userId);
		if (existing == null)
		{
			return NotFound();
		}
		existing.Name = category.Name;
		await _dbContext.SaveChangesAsync();
		return RedirectToAction(nameof(Index));
	}

	public async Task<IActionResult> Delete(int id)
	{
		var userId = GetUserId();
		var category = await FindCategoryForUserAsync(id, userId);
		if (category == null)
		{
			return NotFound();
		}
		return View(category);
	}

	[HttpPost, ActionName("Delete")]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> DeleteConfirmed(int id)
	{
		var userId = GetUserId();
		var category = await FindCategoryForUserAsync(id, userId);
		if (category == null)
		{
			return NotFound();
		}
		_dbContext.Categories.Remove(category);
		await _dbContext.SaveChangesAsync();
		return RedirectToAction(nameof(Index));
	}

	// mevcut kullanıcının kimliğini döndürür
	private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

	// kategori sadece ilgili kullanıcıya aitse bulunur
	private Task<Category?> FindCategoryForUserAsync(int id, string userId)
	{
		return _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
	}
}


