using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mas.Data;
using mas.Models;

namespace mas.Controllers;

[Authorize(Policy = "AdminOnly")]
[Route("api/[controller]")]
[ApiController]
public class PagesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PagesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Pages
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Page>>> GetPages([FromQuery] bool? isPublished = null)
    {
        var query = _context.Pages.AsQueryable();

        if (isPublished.HasValue)
        {
            query = query.Where(p => p.IsPublished == isPublished.Value);
        }

        return await query.OrderBy(p => p.DisplayOrder).ToListAsync();
    }

    // GET: api/Pages/5
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<Page>> GetPage(int id)
    {
        var page = await _context.Pages.FindAsync(id);

        if (page == null)
        {
            return NotFound();
        }

        return page;
    }

    // GET: api/Pages/slug/about-us
    [AllowAnonymous]
    [HttpGet("slug/{slug}")]
    public async Task<ActionResult<Page>> GetPageBySlug(string slug)
    {
        var page = await _context.Pages.FirstOrDefaultAsync(p => p.Slug == slug);

        if (page == null)
        {
            return NotFound();
        }

        if (!page.IsPublished && !User.IsInRole("Admin"))
        {
            return NotFound();
        }

        return page;
    }

    // PUT: api/Pages/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPage(int id, Page page)
    {
        if (id != page.Id)
        {
            return BadRequest();
        }

        page.UpdatedAt = DateTime.UtcNow;
        _context.Entry(page).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PageExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Pages
    [HttpPost]
    public async Task<ActionResult<Page>> PostPage(Page page)
    {
        // Check if slug is unique
        if (await _context.Pages.AnyAsync(p => p.Slug == page.Slug))
        {
            return BadRequest(new { message = "الرابط المخصص (Slug) موجود مسبقاً" });
        }

        _context.Pages.Add(page);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPage), new { id = page.Id }, page);
    }

    // DELETE: api/Pages/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePage(int id)
    {
        var page = await _context.Pages.FindAsync(id);
        if (page == null)
        {
            return NotFound();
        }

        _context.Pages.Remove(page);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool PageExists(int id)
    {
        return _context.Pages.Any(e => e.Id == id);
    }
}
