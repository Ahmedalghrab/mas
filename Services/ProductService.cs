using AutoMapper;
using mas.DTOs;
using mas.Models;
using mas.Repositories;
using Microsoft.EntityFrameworkCore;

namespace mas.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductService> _logger;
    private readonly ICacheService _cacheService;

    public ProductService(
        IUnitOfWork unitOfWork, 
        IMapper mapper, 
        ILogger<ProductService> logger,
        ICacheService cacheService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _cacheService = cacheService;
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync(bool includeInactive = false)
    {
        var cacheKey = $"products_all_{includeInactive}";
        
        return await _cacheService.GetOrCreateAsync(cacheKey, async () =>
        {
            var products = includeInactive 
                ? await _unitOfWork.Products.GetAllAsync()
                : await _unitOfWork.Products.FindAsync(p => p.IsActive);

            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }, TimeSpan.FromMinutes(10));
    }

    public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId)
    {
        var cacheKey = $"products_category_{categoryId}";
        
        return await _cacheService.GetOrCreateAsync(cacheKey, async () =>
        {
            var products = await _unitOfWork.Products.FindAsync(p => p.CategoryId == categoryId && p.IsActive);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }, TimeSpan.FromMinutes(10));
    }

    public async Task<IEnumerable<ProductDto>> GetFeaturedProductsAsync()
    {
        var cacheKey = "products_featured";
        
        return await _cacheService.GetOrCreateAsync(cacheKey, async () =>
        {
            var products = await _unitOfWork.Products.FindAsync(p => p.IsFeatured && p.IsActive);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }, TimeSpan.FromMinutes(15));
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        var cacheKey = $"product_{id}";
        
        return await _cacheService.GetOrCreateAsync(cacheKey, async () =>
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            return product != null ? _mapper.Map<ProductDto>(product) : null;
        }, TimeSpan.FromMinutes(30));
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductDto dto)
    {
        var product = _mapper.Map<Product>(dto);
        
        await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Product created: {ProductId} - {ProductName}", product.Id, product.NameAr);
        
        // Clear cache
        await _cacheService.RemoveByPrefixAsync("products_");
        
        return _mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto> UpdateProductAsync(UpdateProductDto dto)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(dto.Id);
        if (product == null)
            throw new KeyNotFoundException($"Product with ID {dto.Id} not found");

        _mapper.Map(dto, product);
        product.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Products.Update(product);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Product updated: {ProductId} - {ProductName}", product.Id, product.NameAr);
        
        // Clear cache
        await _cacheService.RemoveAsync($"product_{dto.Id}");
        await _cacheService.RemoveByPrefixAsync("products_");
        
        return _mapper.Map<ProductDto>(product);
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null)
            throw new KeyNotFoundException($"Product with ID {id} not found");

        _unitOfWork.Products.Remove(product);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Product deleted: {ProductId}", id);
        
        // Clear cache
        await _cacheService.RemoveAsync($"product_{id}");
        await _cacheService.RemoveByPrefixAsync("products_");
    }

    public async Task<IEnumerable<ProductDto>> SearchProductsAsync(string searchTerm)
    {
        var products = await _unitOfWork.Products.FindAsync(p => 
            p.IsActive && 
            (p.NameAr.Contains(searchTerm) || 
             p.NameEn.Contains(searchTerm) || 
             p.DescriptionAr.Contains(searchTerm)));

        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }
}
