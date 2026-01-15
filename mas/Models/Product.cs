namespace mas.Models;

public class Product
{
    public int Id { get; set; }
    public required string NameAr { get; set; } // Arabic name
    public required string NameEn { get; set; } // English name
 public required string DescriptionAr { get; set; }
    public string? DescriptionEn { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public string? ImagePath { get; set; }
    public string? OriginalImagePath { get; set; } // Original image before AI enhancement
    public string? ThumbnailPath { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsFeatured { get; set; } = false;
    public int DisplayOrder { get; set; } = 0;
    public int CategoryId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Service-specific properties
    public int? DeliveryTimeDays { get; set; }
    public string? WhatsAppNumber { get; set; }
    public string? EmailContact { get; set; }

    // Navigation property
    public Category Category { get; set; } = null!;
}
