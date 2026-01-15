namespace mas.Models;

/// <summary>
/// محتوى صفحة "من نحن" - قابل للتعديل من لوحة التحكم
/// </summary>
public class AboutContent
{
    public int Id { get; set; }
    
    // Main Content
    public string? TitleAr { get; set; }
    public string? TitleEn { get; set; }
    public string? SubtitleAr { get; set; }
    public string? SubtitleEn { get; set; }
    public string? MainContentAr { get; set; }
    public string? MainContentEn { get; set; }
    public string? MainImagePath { get; set; }
    
    // Vision
    public string? VisionTitleAr { get; set; }
    public string? VisionTitleEn { get; set; }
    public string? VisionContentAr { get; set; }
    public string? VisionContentEn { get; set; }
    public string? VisionIcon { get; set; }
    
    // Mission
    public string? MissionTitleAr { get; set; }
    public string? MissionTitleEn { get; set; }
    public string? MissionContentAr { get; set; }
    public string? MissionContentEn { get; set; }
    public string? MissionIcon { get; set; }
    
    // Values
    public string? ValuesTitleAr { get; set; }
    public string? ValuesTitleEn { get; set; }
    
    public string? Value1TitleAr { get; set; }
    public string? Value1TitleEn { get; set; }
    public string? Value1DescriptionAr { get; set; }
    public string? Value1DescriptionEn { get; set; }
    public string? Value1Icon { get; set; }
    
    public string? Value2TitleAr { get; set; }
    public string? Value2TitleEn { get; set; }
    public string? Value2DescriptionAr { get; set; }
    public string? Value2DescriptionEn { get; set; }
    public string? Value2Icon { get; set; }
    
    public string? Value3TitleAr { get; set; }
    public string? Value3TitleEn { get; set; }
    public string? Value3DescriptionAr { get; set; }
    public string? Value3DescriptionEn { get; set; }
    public string? Value3Icon { get; set; }
    
    // Team Section
    public string? TeamSectionTitleAr { get; set; }
    public string? TeamSectionTitleEn { get; set; }
    public string? TeamSectionDescriptionAr { get; set; }
    public string? TeamSectionDescriptionEn { get; set; }
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
