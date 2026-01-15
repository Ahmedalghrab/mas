using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace mas.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImageController : ControllerBase
{
private readonly IWebHostEnvironment _environment;
    private readonly ILogger<ImageController> _logger;
    private const int MaxFileSize = 10 * 1024 * 1024; // 10MB
 private readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

    public ImageController(IWebHostEnvironment environment, ILogger<ImageController> logger)
    {
        _environment = environment;
        _logger = logger;
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost("upload")]
    public async Task<ActionResult<ImageUploadResponse>> UploadImage(IFormFile file, [FromQuery] bool enhance = true)
    {
        if (file == null || file.Length == 0)
    return BadRequest("No file uploaded");

        if (file.Length > MaxFileSize)
       return BadRequest("File size exceeds 10MB limit");

var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(extension))
         return BadRequest("Invalid file type. Only images are allowed.");

        try
        {
   var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "products");
     var thumbnailFolder = Path.Combine(_environment.WebRootPath, "uploads", "thumbnails");
        
      Directory.CreateDirectory(uploadsFolder);
            Directory.CreateDirectory(thumbnailFolder);

      var fileName = $"{Guid.NewGuid()}{extension}";
      var filePath = Path.Combine(uploadsFolder, fileName);
     var thumbnailPath = Path.Combine(thumbnailFolder, fileName);

    using (var image = await Image.LoadAsync(file.OpenReadStream()))
       {
        // Save original or enhanced version
                if (enhance)
                {
     // Apply AI-like enhancements
           image.Mutate(x => x
  .AutoOrient() // Fix orientation
      .Resize(new ResizeOptions
 {
          Size = new Size(1200, 1200),
            Mode = ResizeMode.Max
       })
       .GaussianSharpen(1.5f) // Sharpen
         .Saturate(1.2f) // Increase saturation
    .Contrast(1.1f) // Increase contrast slightly
   .Brightness(1.05f) // Slightly brighten
          );
     }
  else
      {
         image.Mutate(x => x
            .AutoOrient()
   .Resize(new ResizeOptions
    {
             Size = new Size(1200, 1200),
       Mode = ResizeMode.Max
        })
            );
          }

    await image.SaveAsync(filePath, new JpegEncoder { Quality = 90 });

// Create thumbnail
           var thumbnailSize = new Size(300, 300);
          image.Mutate(x => x.Resize(new ResizeOptions
         {
     Size = thumbnailSize,
     Mode = ResizeMode.Crop
                }));

   await image.SaveAsync(thumbnailPath, new JpegEncoder { Quality = 85 });
 }

            return Ok(new ImageUploadResponse
   {
            ImagePath = $"/uploads/products/{fileName}",
     ThumbnailPath = $"/uploads/thumbnails/{fileName}",
                FileName = fileName
  });
        }
        catch (Exception ex)
    {
    _logger.LogError(ex, "Error uploading image");
  return StatusCode(500, "An error occurred while processing the image");
        }
 }

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("delete")]
  public IActionResult DeleteImage([FromQuery] string imagePath)
    {
        if (string.IsNullOrEmpty(imagePath))
    return BadRequest("Image path is required");

  try
        {
   var fullPath = Path.Combine(_environment.WebRootPath, imagePath.TrimStart('/'));
            
     if (System.IO.File.Exists(fullPath))
            {
     System.IO.File.Delete(fullPath);
                return Ok("Image deleted successfully");
            }

   return NotFound("Image not found");
 }
        catch (Exception ex)
{
         _logger.LogError(ex, "Error deleting image");
 return StatusCode(500, "An error occurred while deleting the image");
     }
    }
}

public class ImageUploadResponse
{
    public required string ImagePath { get; set; }
    public required string ThumbnailPath { get; set; }
    public required string FileName { get; set; }
}
