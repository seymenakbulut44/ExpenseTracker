using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Models;

namespace ExpenseTracker.Data;

public class ApplicationDbContext : IdentityDbContext
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options)
	{
	}

	// kullanıcıya ait kategoriler ve işlemler için dbset tanımları
	public DbSet<Category> Categories => Set<Category>();
	public DbSet<Transaction> Transactions => Set<Transaction>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		// model yapılandırmalarını küçük parçalara ayırmak okunabilirliği artırır
		ConfigureCategory(modelBuilder);
		ConfigureTransaction(modelBuilder);
	}

	// kategori için benzersiz indeks ve gerekli alanlar
	private static void ConfigureCategory(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Category>()
			.HasIndex(c => new { c.UserId, c.Name })
			.IsUnique();

		modelBuilder.Entity<Category>()
			.Property(c => c.UserId)
			.IsRequired();
	}

	// işlem için ilişki, gerekli alanlar ve tutar hassasiyeti
    private static void ConfigureTransaction(ModelBuilder modelBuilder)
    {
    	modelBuilder.Entity<Transaction>()
    		.HasOne(t => t.Category)
    		.WithMany()
    		.HasForeignKey(t => t.CategoryId)
    		.OnDelete(DeleteBehavior.Restrict);

    	modelBuilder.Entity<Transaction>()
    		.Property(t => t.UserId)
    		.IsRequired();

    	modelBuilder.Entity<Transaction>()
    		.Property(t => t.Amount)
    		.HasPrecision(18, 2);
    }
}
