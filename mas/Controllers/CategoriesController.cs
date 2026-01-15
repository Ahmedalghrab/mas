using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mas.Data;
using mas.Models;

namespace mas.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CategoriesController(ApplicationDbContext context)
    {
 _context = context;
    }

    // GET: api/Categories
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories([FromQuery] bool? isActive = true)
    {
        var query = _context.Categories.AsQueryable();

        if (isActive.HasValue)
       query = query.Where(c => c.IsActive == isActive.Value);

        return await query.OrderBy(c => c.NameAr).ToListAsync();
    }

    // GET: api/Categories/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategory(int id)
    {
      var category = await _context.Categories
            .Include(c => c.Products)
        .FirstOrDefaultAsync(c => c.Id == id);

      if (category == null)
  return NotFound();

        return category;
    }

    // POST: api/Categories
    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    public async Task<ActionResult<Category>> CreateCategory(Category category)
  {
        category.CreatedAt = DateTime.UtcNow;
    _context.Categories.Add(category);
        await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
    }

    // PUT: api/Categories/5
 [Authorize(Policy = "AdminOnly")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, Category category)
    {
        if (id != category.Id)
          return BadRequest();

        _context.Entry(category).State = EntityState.Modified;

      try
        {
            await _context.SaveChangesAsync();
    }
        catch (DbUpdateConcurrencyException)
        {
            if (!await CategoryExists(id))
return NotFound();
   throw;
        }

        return NoContent();
  }

    // DELETE: api/Categories/5
    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
          return NotFound();

    _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> CategoryExists(int id)
    {
        return await _context.Categories.AnyAsync(e => e.Id == id);
    }
}
