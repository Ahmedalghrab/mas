namespace mas.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public string DescriptionAr { get; set; } = null!;
    public string? DescriptionEn { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public string? ImagePath { get; set; }
    public string? ThumbnailPath { get; set; }
    public bool IsActive { get; set; }
    public bool IsFeatured { get; set; }
    public int CategoryId { get; set; }
    public string CategoryNameAr { get; set; } = null!;
    public string CategoryNameEn { get; set; } = null!;
    public int? DeliveryTimeDays { get; set; }
    public double AverageRating { get; set; }
    public int ReviewCount { get; set; }
}

public class CreateProductDto
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public string DescriptionAr { get; set; } = null!;
    public string? DescriptionEn { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public int CategoryId { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsFeatured { get; set; }
    public int? DeliveryTimeDays { get; set; }
    public string? WhatsAppNumber { get; set; }
    public string? EmailContact { get; set; }
}

public class UpdateProductDto : CreateProductDto
{
    public int Id { get; set; }
}
