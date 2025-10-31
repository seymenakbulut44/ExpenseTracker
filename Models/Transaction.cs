using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models;

public enum TransactionType
{
	Expense = 0,
	Income = 1
}

public class Transaction
{
	public int Id { get; set; }

	[Required]
	public decimal Amount { get; set; }

	[Required]
	public DateTime Date { get; set; } = DateTime.UtcNow;

	[StringLength(250)]
	public string? Notes { get; set; }

	[Required]
	public TransactionType Type { get; set; }

	public int CategoryId { get; set; }
	public Category? Category { get; set; }

	public string UserId { get; set; } = string.Empty;
}


