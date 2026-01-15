using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mas.Data;
using mas.Models;

namespace mas.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HomeContentController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public HomeContentController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/HomeContent
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<HomeContent>> GetHomeContent()
    {
        var content = await _context.HomeContents.FirstOrDefaultAsync();
        
        if (content == null)
        {
            // Create default content if not exists
            content = new HomeContent
            {
                HeroTitleAr = "مرحباً بك في الماسة للخدمات الطلابية",
                HeroTitleEn = "Welcome to Almasa Student Services",
                HeroSubtitleAr = "نقدم لك أفضل الخدمات الأكاديمية والتقنية بجودة عالية",
                HeroSubtitleEn = "We provide you with the best academic and technical services with high quality",
                HeroButtonTextAr = "استكشف خدماتنا",
                HeroButtonTextEn = "Explore Our Services",
                HeroButtonLink = "/services"
            };
            _context.HomeContents.Add(content);
            await _context.SaveChangesAsync();
        }

        return content;
    }

    // PUT: api/HomeContent
    [Authorize(Policy = "AdminOnly")]
    [HttpPut]
    public async Task<IActionResult> PutHomeContent(HomeContent content)
    {
        var existingContent = await _context.HomeContents.FirstOrDefaultAsync();

        if (existingContent == null)
        {
            content.UpdatedAt = DateTime.UtcNow;
            _context.HomeContents.Add(content);
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
