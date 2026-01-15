namespace mas.Models;

/// <summary>
/// محتوى صفحة "اتصل بنا" - قابل للتعديل من لوحة التحكم
/// </summary>
public class ContactContent
{
    public int Id { get; set; }
    
    // Page Header
    public string? PageTitleAr { get; set; }
    public string? PageTitleEn { get; set; }
    public string? PageSubtitleAr { get; set; }
    public string? PageSubtitleEn { get; set; }
    
    // Contact Information
    public string? AddressAr { get; set; }
    public string? AddressEn { get; set; }
    public string? PhoneNumber { get; set; }
    public string? EmailAddress { get; set; }
    public string? WhatsAppNumber { get; set; }
    
    // Working Hours
    public string? WorkingHoursTitleAr { get; set; }
    public string? WorkingHoursTitleEn { get; set; }
    public string? WorkingHoursAr { get; set; }
    public string? WorkingHoursEn { get; set; }
    
    // Map
    public string? MapEmbedUrl { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    
    // Form Settings
    public string? FormTitleAr { get; set; }
    public string? FormTitleEn { get; set; }
    public string? FormDescriptionAr { get; set; }
    public string? FormDescriptionEn { get; set; }
    public string? SuccessMessageAr { get; set; }
    public string? SuccessMessageEn { get; set; }
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
