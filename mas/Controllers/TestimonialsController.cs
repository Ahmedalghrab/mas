using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mas.Data;
using mas.Models;

namespace mas.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestimonialsController : ControllerBase
{
 private readonly ApplicationDbContext _context;

 public TestimonialsController(ApplicationDbContext context)
    {
   _context = context;
    }

    // GET: api/Testimonials
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Testimonial>>> GetTestimonials([FromQuery] bool? isActive = true)
    {
 var query = _context.Testimonials.AsQueryable();

    if (isActive.HasValue)
            query = query.Where(t => t.IsActive == isActive.Value);

return await query.OrderBy(t => t.DisplayOrder).ToListAsync();
    }

    // GET: api/Testimonials/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Testimonial>> GetTestimonial(int id)
    {
        var testimonial = await _context.Testimonials.FindAsync(id);
     if (testimonial == null)
       return NotFound();
        return testimonial;
    }

    // POST: api/Testimonials
  [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    public async Task<ActionResult<Testimonial>> CreateTestimonial(Testimonial testimonial)
    {
        testimonial.CreatedAt = DateTime.UtcNow;
        _context.Testimonials.Add(testimonial);
        await _context.SaveChangesAsync();
      return CreatedAtAction(nameof(GetTestimonial), new { id = testimonial.Id }, testimonial);
    }

    // PUT: api/Testimonials/5
    [Authorize(Policy = "AdminOnly")]
    [HttpPut("{id}")]
  public async Task<IActionResult> UpdateTestimonial(int id, Testimonial testimonial)
    {
  if (id != testimonial.Id)
            return BadRequest();

        _context.Entry(testimonial).State = EntityState.Modified;

        try
   {
 await _context.SaveChangesAsync();
     }
   catch (DbUpdateConcurrencyException)
        {
       if (!await TestimonialExists(id))
      return NotFound();
    throw;
        }

        return NoContent();
 }

    // DELETE: api/Testimonials/5
    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTestimonial(int id)
    {
        var testimonial = await _context.Testimonials.FindAsync(id);
 if (testimonial == null)
  return NotFound();

_context.Testimonials.Remove(testimonial);
 await _context.SaveChangesAsync();
   return NoContent();
 }

    private async Task<bool> TestimonialExists(int id)
    {
        return await _context.Testimonials.AnyAsync(e => e.Id == id);
    }
}
