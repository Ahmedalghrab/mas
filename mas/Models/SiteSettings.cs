namespace mas.Models;

public class SiteSettings
{
    public int Id { get; set; }
    public string SiteName { get; set; } = "ALMASS";
    public string SiteNameEn { get; set; } = "ALMASS";
    public string? LogoPath { get; set; }
    public string? FaviconPath { get; set; }
    
    // Contact Information
    public string? WhatsAppNumber { get; set; }
    public string? PhoneNumber { get; set; }
  public string? Email { get; set; }
    public string? Address { get; set; }
    
    // Social Media
    public string? FacebookUrl { get; set; }
    public string? TwitterUrl { get; set; }
    public string? InstagramUrl { get; set; }
    public string? LinkedInUrl { get; set; }
    
    // About/Overview
    public string? AboutAr { get; set; }
 public string? AboutEn { get; set; }
    public string? VisionAr { get; set; }
    public string? VisionEn { get; set; }
    public string? MissionAr { get; set; }
    public string? MissionEn { get; set; }
    
  // SEO
    public string? MetaDescriptionAr { get; set; }
    public string? MetaDescriptionEn { get; set; }
    public string? MetaKeywords { get; set; }
    
  // Features
    public bool EnableWhatsAppButton { get; set; } = true;
    public bool EnableDarkMode { get; set; } = true;
    public bool EnableTestimonials { get; set; } = true;
    public bool MaintenanceMode { get; set; } = false;
    
    // Colors
    public string? PrimaryColor { get; set; } = "#0d6efd";
    public string? SecondaryColor { get; set; } = "#6c757d";
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
