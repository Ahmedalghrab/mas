using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mas.Services;

namespace mas.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImageController : ControllerBase
{
    private readonly IImageUploadService _imageUploadService;
    private readonly ILogger<ImageController> _logger;
    private const int MaxFileSize = 10 * 1024 * 1024; // 10MB
    private readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

    public ImageController(IImageUploadService imageUploadService, ILogger<ImageController> logger)
    {
        _imageUploadService = imageUploadService;
        _logger = logger;
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost("upload")]
    public async Task<ActionResult<ImageUploadResponse>> UploadImage(IFormFile file, [FromQuery] string folder = "products")
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
            // Upload to Cloudinary
            var imageUrl = await _imageUploadService.UploadImageAsync(file, folder);

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

            return Ok(new ImageUploadResponse
            {
                ImagePath = imageUrl,
                ThumbnailPath = imageUrl, // Cloudinary handles thumbnails automatically
                FileName = Path.GetFileName(file.FileName)
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
    public async Task<IActionResult> DeleteImage([FromQuery] string publicId)
    {
        if (string.IsNullOrEmpty(publicId))
            return BadRequest("Public ID is required");

        try
        {
            var deleted = await _imageUploadService.DeleteImageAsync(publicId);
            if (deleted)
                return Ok("Image deleted successfully");
            
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
