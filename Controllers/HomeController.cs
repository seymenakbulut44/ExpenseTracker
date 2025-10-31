using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Models;
using ExpenseTracker.Data;

namespace ExpenseTracker.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _dbContext;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Index()
    {
        var userId = GetUserId();

        // ilk girişte temel kategorileri tek seferlik oluştur
        await SeedDefaultCategoriesAsync(userId);

        var transactions = await _dbContext.Transactions
            .Include(t => t.Category)
            .Where(t => t.UserId == userId)
            .ToListAsync();

        var totalIncome = transactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
        var totalExpense = transactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount);
        var balance = totalIncome - totalExpense;

        var expenseByCategory = transactions
            .Where(t => t.Type == TransactionType.Expense)
            .GroupBy(t => t.Category!.Name)
            .Select(g => new { Category = g.Key, Amount = g.Sum(x => x.Amount) })
            .OrderByDescending(x => x.Amount)
            .ToList();

        ViewBag.TotalIncome = totalIncome;
        ViewBag.TotalExpense = totalExpense;
        ViewBag.Balance = balance;
        ViewBag.ChartLabels = expenseByCategory.Select(x => x.Category).ToArray();
        ViewBag.ChartData = expenseByCategory.Select(x => x.Amount).ToArray();

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    // mevcut kullanıcı id bilgisini almak için küçük yardımcı
    private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    // kullanıcıya başlangıç kategorilerini ekler
    private async Task SeedDefaultCategoriesAsync(string userId)
    {
        var hasAny = await _dbContext.Categories.AnyAsync(c => c.UserId == userId);
        if (hasAny)
        {
            return;
        }

        _dbContext.Categories.AddRange(new[]
        {
            new Category { Name = "Food", UserId = userId },
            new Category { Name = "Rent", UserId = userId },
            new Category { Name = "Transport", UserId = userId },
            new Category { Name = "Utilities", UserId = userId },
            new Category { Name = "Salary", UserId = userId }
        });
        await _dbContext.SaveChangesAsync();
    }
}
