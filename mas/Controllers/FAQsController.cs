using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mas.Data;
using mas.Models;

namespace mas.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FAQsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public FAQsController(ApplicationDbContext context)
    {
  _context = context;
    }

    // GET: api/FAQs
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FAQ>>> GetFAQs([FromQuery] bool? isActive = true)
    {
        var query = _context.FAQs.AsQueryable();

   if (isActive.HasValue)
    query = query.Where(f => f.IsActive == isActive.Value);

        return await query.OrderBy(f => f.DisplayOrder).ToListAsync();
    }

    // GET: api/FAQs/5
    [HttpGet("{id}")]
    public async Task<ActionResult<FAQ>> GetFAQ(int id)
    {
        var faq = await _context.FAQs.FindAsync(id);
   if (faq == null)
     return NotFound();
     return faq;
    }

 // POST: api/FAQs
  [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    public async Task<ActionResult<FAQ>> CreateFAQ(FAQ faq)
    {
        faq.CreatedAt = DateTime.UtcNow;
     _context.FAQs.Add(faq);
        await _context.SaveChangesAsync();
     return CreatedAtAction(nameof(GetFAQ), new { id = faq.Id }, faq);
    }

    // PUT: api/FAQs/5
    [Authorize(Policy = "AdminOnly")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFAQ(int id, FAQ faq)
  {
   if (id != faq.Id)
 return BadRequest();

  _context.Entry(faq).State = EntityState.Modified;

     try
        {
     await _context.SaveChangesAsync();
    }
   catch (DbUpdateConcurrencyException)
  {
     if (!await FAQExists(id))
   return NotFound();
          throw;
  }

        return NoContent();
  }

    // DELETE: api/FAQs/5
    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFAQ(int id)
    {
    var faq = await _context.FAQs.FindAsync(id);
 if (faq == null)
  return NotFound();

        _context.FAQs.Remove(faq);
   await _context.SaveChangesAsync();
     return NoContent();
    }

 private async Task<bool> FAQExists(int id)
    {
    return await _context.FAQs.AnyAsync(e => e.Id == id);
    }
}
