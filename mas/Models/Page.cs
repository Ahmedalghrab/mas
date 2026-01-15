namespace mas.Models;

/// <summary>
/// نموذج الصفحات الديناميكية - يسمح بإنشاء صفحات جديدة من لوحة التحكم
/// </summary>
public class Page
{
    public int Id { get; set; }
    
    /// <summary>
    /// عنوان الصفحة بالعربية
    /// </summary>
    public required string TitleAr { get; set; }
    
    /// <summary>
    /// عنوان الصفحة بالإنجليزية
    /// </summary>
    public string? TitleEn { get; set; }
    
    /// <summary>
    /// الرابط الفريد للصفحة (مثل: about-us, privacy-policy)
    /// </summary>
    public required string Slug { get; set; }
    
    /// <summary>
    /// المحتوى بالعربية (يدعم HTML)
    /// </summary>
    public required string ContentAr { get; set; }
    
    /// <summary>
    /// المحتوى بالإنجليزية
    /// </summary>
    public string? ContentEn { get; set; }
    
    /// <summary>
    /// وصف قصير للصفحة (SEO)
    /// </summary>
    public string? MetaDescriptionAr { get; set; }
    public string? MetaDescriptionEn { get; set; }
    
    /// <summary>
    /// الكلمات المفتاحية (SEO)
    /// </summary>
    public string? MetaKeywords { get; set; }
    
    /// <summary>
    /// صورة مميزة للصفحة
    /// </summary>
    public string? FeaturedImagePath { get; set; }
    
    /// <summary>
    /// ترتيب الظهور في القائمة
    /// </summary>
    public int DisplayOrder { get; set; } = 0;
    
    /// <summary>
    /// هل الصفحة منشورة؟
    /// </summary>
    public bool IsPublished { get; set; } = true;
    
    /// <summary>
    /// هل تظهر في القائمة الرئيسية؟
    /// </summary>
    public bool ShowInMenu { get; set; } = false;
    
    /// <summary>
    /// أيقونة الصفحة (Bootstrap Icons)
    /// </summary>
    public string? IconClass { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
