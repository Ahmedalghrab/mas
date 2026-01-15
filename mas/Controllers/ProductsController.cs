using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mas.Data;
using mas.Models;

namespace mas.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(ApplicationDbContext context, ILogger<ProductsController> logger)
    {
     _context = context;
     _logger = logger;
    }

    // GET: api/Products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(
   [FromQuery] int? categoryId = null,
        [FromQuery] bool? isActive = true,
     [FromQuery] bool? isFeatured = null)
    {
        var query = _context.Products.Include(p => p.Category).AsQueryable();

        if (categoryId.HasValue)
       query = query.Where(p => p.CategoryId == categoryId.Value);

        if (isActive.HasValue)
         query = query.Where(p => p.IsActive == isActive.Value);

        if (isFeatured.HasValue)
            query = query.Where(p => p.IsFeatured == isFeatured.Value);

        return await query.OrderBy(p => p.DisplayOrder).ToListAsync();
    }

    // GET: api/Products/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
    .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            return NotFound();

        return product;
    }

    // POST: api/Products
    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
  product.CreatedAt = DateTime.UtcNow;
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

      return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    // PUT: api/Products/5
    [Authorize(Policy = "AdminOnly")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, Product product)
    {
        if (id != product.Id)
  return BadRequest();

     product.UpdatedAt = DateTime.UtcNow;
 _context.Entry(product).State = EntityState.Modified;

 try
        {
 await _context.SaveChangesAsync();
        }
   catch (DbUpdateConcurrencyException)
        {
            if (!await ProductExists(id))
         return NotFound();
          throw;
        }

        return NoContent();
    }

    // DELETE: api/Products/5
    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
 var product = await _context.Products.FindAsync(id);
    if (product == null)
            return NotFound();

   _context.Products.Remove(product);
    await _context.SaveChangesAsync();

      return NoContent();
    }

    // GET: api/Products/Featured
 [HttpGet("featured")]
    public async Task<ActionResult<IEnumerable<Product>>> GetFeaturedProducts()
    {
        return await _context.Products
    .Include(p => p.Category)
         .Where(p => p.IsActive && p.IsFeatured)
     .OrderBy(p => p.DisplayOrder)
     .Take(6)
        .ToListAsync();
    }

    private async Task<bool> ProductExists(int id)
    {
        return await _context.Products.AnyAsync(e => e.Id == id);
    }
}
