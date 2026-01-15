namespace mas.Services;

public interface IImageUploadService
{
    Task<string> UploadImageAsync(IFormFile file, string folder = "products");
    Task<bool> DeleteImageAsync(string publicId);
}
