using mas.DTOs;

namespace mas.Services;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllProductsAsync(bool includeInactive = false);
    Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId);
    Task<IEnumerable<ProductDto>> GetFeaturedProductsAsync();
    Task<ProductDto?> GetProductByIdAsync(int id);
    Task<ProductDto> CreateProductAsync(CreateProductDto dto);
    Task<ProductDto> UpdateProductAsync(UpdateProductDto dto);
    Task DeleteProductAsync(int id);
    Task<IEnumerable<ProductDto>> SearchProductsAsync(string searchTerm);
}
