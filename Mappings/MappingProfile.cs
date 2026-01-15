using AutoMapper;
using mas.DTOs;
using mas.Models;

namespace mas.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Product Mappings
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.CategoryNameAr, opt => opt.MapFrom(src => src.Category.NameAr))
            .ForMember(dest => dest.CategoryNameEn, opt => opt.MapFrom(src => src.Category.NameEn))
            .ForMember(dest => dest.AverageRating, opt => opt.Ignore())
            .ForMember(dest => dest.ReviewCount, opt => opt.Ignore());

        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();

        // Order Mappings
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName));
        CreateMap<OrderItem, OrderItemDto>();
        CreateMap<CreateOrderDto, Order>();

        // Review Mappings
        CreateMap<Review, ReviewDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName));
        CreateMap<CreateReviewDto, Review>();

        // Category Mappings
        CreateMap<Category, CategoryDto>();
        CreateMap<CreateCategoryDto, Category>();
    }
}
