namespace mas.Models;

/// <summary>
/// محتوى الصفحة الرئيسية - قابل للتعديل من لوحة التحكم
/// </summary>
public class HomeContent
{
    public int Id { get; set; }
    
    // Hero Section
    public string? HeroTitleAr { get; set; }
    public string? HeroTitleEn { get; set; }
    public string? HeroSubtitleAr { get; set; }
    public string? HeroSubtitleEn { get; set; }
    public string? HeroImagePath { get; set; }
    public string? HeroButtonTextAr { get; set; }
    public string? HeroButtonTextEn { get; set; }
    public string? HeroButtonLink { get; set; }
    
    // Features Section
    public string? FeaturesSectionTitleAr { get; set; }
    public string? FeaturesSectionTitleEn { get; set; }
    
    public string? Feature1TitleAr { get; set; }
    public string? Feature1TitleEn { get; set; }
    public string? Feature1DescriptionAr { get; set; }
    public string? Feature1DescriptionEn { get; set; }
    public string? Feature1Icon { get; set; }
    
    public string? Feature2TitleAr { get; set; }
    public string? Feature2TitleEn { get; set; }
    public string? Feature2DescriptionAr { get; set; }
    public string? Feature2DescriptionEn { get; set; }
    public string? Feature2Icon { get; set; }
    
    public string? Feature3TitleAr { get; set; }
    public string? Feature3TitleEn { get; set; }
    public string? Feature3DescriptionAr { get; set; }
    public string? Feature3DescriptionEn { get; set; }
    public string? Feature3Icon { get; set; }
    
    public string? Feature4TitleAr { get; set; }
    public string? Feature4TitleEn { get; set; }
    public string? Feature4DescriptionAr { get; set; }
    public string? Feature4DescriptionEn { get; set; }
    public string? Feature4Icon { get; set; }
    
    // Services Section
    public string? ServicesSectionTitleAr { get; set; }
    public string? ServicesSectionTitleEn { get; set; }
    public string? ServicesSectionSubtitleAr { get; set; }
    public string? ServicesSectionSubtitleEn { get; set; }
    
    // About Section
    public string? AboutSectionTitleAr { get; set; }
    public string? AboutSectionTitleEn { get; set; }
    public string? AboutSectionContentAr { get; set; }
    public string? AboutSectionContentEn { get; set; }
    public string? AboutImagePath { get; set; }
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
