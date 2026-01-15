namespace mas.Models;

public class Category
{
    public int Id { get; set; }
    public required string NameAr { get; set; } // Arabic name
    public required string NameEn { get; set; } // English name (optional)
    public string? DescriptionAr { get; set; }
    public string? DescriptionEn { get; set; }
    public string? IconClass { get; set; } // CSS icon class
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation property
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
