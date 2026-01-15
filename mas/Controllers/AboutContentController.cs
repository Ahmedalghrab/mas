using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mas.Data;
using mas.Models;

namespace mas.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AboutContentController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AboutContentController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/AboutContent
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<AboutContent>> GetAboutContent()
    {
        var content = await _context.AboutContents.FirstOrDefaultAsync();
        
        if (content == null)
        {
            // Create default content
            content = new AboutContent
            {
                TitleAr = "من نحن",
                TitleEn = "About Us",
                MainContentAr = "نحن منصة متخصصة في تقديم الخدمات الأكاديمية والتقنية للطلاب بجودة عالية وأسعار مناسبة",
                MainContentEn = "We are a platform specialized in providing academic and technical services to students with high quality and reasonable prices",
                VisionTitleAr = "رؤيتنا",
                VisionTitleEn = "Our Vision",
                VisionContentAr = "أن نكون المنصة الأولى في تقديم الخدمات الأكاديمية المتميزة",
                VisionContentEn = "To be the first platform in providing distinguished academic services",
                MissionTitleAr = "رسالتنا",
                MissionTitleEn = "Our Mission",
                MissionContentAr = "تقديم خدمات عالية الجودة تساعد الطلاب على تحقيق أهدافهم الأكاديمية",
                MissionContentEn = "Providing high quality services that help students achieve their academic goals"
            };
            _context.AboutContents.Add(content);
            await _context.SaveChangesAsync();
        }

        return content;
    }

    // PUT: api/AboutContent
    [Authorize(Policy = "AdminOnly")]
    [HttpPut]
    public async Task<IActionResult> PutAboutContent(AboutContent content)
    {
        var existingContent = await _context.AboutContents.FirstOrDefaultAsync();

        if (existingContent == null)
        {
            content.UpdatedAt = DateTime.UtcNow;
            _context.AboutContents.Add(content);
        }
        else
        {
            content.Id = existingContent.Id;
            content.UpdatedAt = DateTime.UtcNow;
            _context.Entry(existingContent).CurrentValues.SetValues(content);
        }

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw;
        }

        return NoContent();
    }
}
