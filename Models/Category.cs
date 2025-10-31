using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models;

public class Category
{
	public int Id { get; set; }

	[Required]
	[StringLength(100)]
	public string Name { get; set; } = string.Empty;

	public string UserId { get; set; } = string.Empty;
}


