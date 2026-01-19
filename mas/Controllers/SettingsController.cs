using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mas.Data;
using mas.Models;

namespace mas.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SettingsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public SettingsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Settings
    [HttpGet]
    public async Task<ActionResult<SiteSettings>> GetSettings()
    {
     var settings = await _context.SiteSettings.FirstOrDefaultAsync();
  if (settings == null)
      {
     // Return default settings if none exist
 settings = new SiteSettings
            {
 SiteName = "ALMASS",
        SiteNameEn = "ALMASS",
            PrimaryColor = "#0d6efd"
        };
  }
        return settings;
    }

    // PUT: api/Settings
    [Authorize(Policy = "AdminOnly")]
    [HttpPut]
    public async Task<IActionResult> UpdateSettings(SiteSettings settings)
    {
var existing = await _context.SiteSettings.FirstOrDefaultAsync();
      
   if (existing == null)
        {
    settings.Id = 1;
     _context.SiteSettings.Add(settings);
}
        else
      {
   existing.SiteName = settings.SiteName;
            existing.SiteNameEn = settings.SiteNameEn;
            existing.LogoPath = settings.LogoPath;
     existing.FaviconPath = settings.FaviconPath;
     existing.WhatsAppNumber = settings.WhatsAppNumber;
        existing.PhoneNumber = settings.PhoneNumber;
         existing.Email = settings.Email;
            existing.Address = settings.Address;
 existing.FacebookUrl = settings.FacebookUrl;
      existing.TwitterUrl = settings.TwitterUrl;
            existing.InstagramUrl = settings.InstagramUrl;
    existing.LinkedInUrl = settings.LinkedInUrl;
       existing.AboutAr = settings.AboutAr;
        existing.AboutEn = settings.AboutEn;
            existing.VisionAr = settings.VisionAr;
  existing.VisionEn = settings.VisionEn;
      existing.MissionAr = settings.MissionAr;
  existing.MissionEn = settings.MissionEn;
     existing.MetaDescriptionAr = settings.MetaDescriptionAr;
            existing.MetaDescriptionEn = settings.MetaDescriptionEn;
            existing.MetaKeywords = settings.MetaKeywords;
       existing.EnableWhatsAppButton = settings.EnableWhatsAppButton;
            existing.EnableDarkMode = settings.EnableDarkMode;
 existing.EnableTestimonials = settings.EnableTestimonials;
            existing.MaintenanceMode = settings.MaintenanceMode;
            existing.PrimaryColor = settings.PrimaryColor;
            existing.SecondaryColor = settings.SecondaryColor;
            existing.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
        return Ok(settings);
    }
}
