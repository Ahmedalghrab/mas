using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using mas.Models;

namespace mas.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
   : base(options)
    {
        // Debug: Print connection string
        var connString = Database.GetConnectionString();
        Console.WriteLine($"[DbContext] Connection String Length: {connString?.Length ?? 0}");
        Console.WriteLine($"[DbContext] FULL Connection String: '{connString}'");
        if (connString != null)
        {
            Console.WriteLine($"[DbContext] First char code: {(int)connString[0]}");
            Console.WriteLine($"[DbContext] Last char code: {(int)connString[connString.Length - 1]}");
        }
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<SiteSettings> SiteSettings { get; set; }
    public DbSet<Testimonial> Testimonials { get; set; }
    public DbSet<FAQ> FAQs { get; set; }
    public DbSet<ContactMessage> ContactMessages { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<MarketingCampaign> MarketingCampaigns { get; set; }
    
    // CMS Content Tables
    public DbSet<Page> Pages { get; set; }
    public DbSet<HomeContent> HomeContents { get; set; }
    public DbSet<AboutContent> AboutContents { get; set; }
    public DbSet<ContactContent> ContactContents { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure Product
     builder.Entity<Product>(entity =>
   {
     entity.HasKey(e => e.Id);
   entity.Property(e => e.NameAr).IsRequired().HasMaxLength(200);
            entity.Property(e => e.NameEn).IsRequired().HasMaxLength(200);
        entity.Property(e => e.DescriptionAr).IsRequired();
            entity.Property(e => e.Price).HasPrecision(18, 2);
            entity.Property(e => e.DiscountPrice).HasPrecision(18, 2);

  entity.HasOne(e => e.Category)
            .WithMany(c => c.Products)
      .HasForeignKey(e => e.CategoryId)
          .OnDelete(DeleteBehavior.Restrict);

     entity.HasIndex(e => e.CategoryId);
          entity.HasIndex(e => e.IsActive);
        entity.HasIndex(e => e.IsFeatured);
    });

      // Configure Category
        builder.Entity<Category>(entity =>
        {
entity.HasKey(e => e.Id);
            entity.Property(e => e.NameAr).IsRequired().HasMaxLength(100);
     entity.Property(e => e.NameEn).IsRequired().HasMaxLength(100);
      });
    }
}
