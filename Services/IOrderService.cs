using mas.DTOs;

namespace mas.Services;

public interface IOrderService
{
    Task<OrderDto> CreateOrderAsync(string userId, CreateOrderDto dto);
    Task<OrderDto?> GetOrderByIdAsync(int id);
    Task<IEnumerable<OrderDto>> GetUserOrdersAsync(string userId);
    Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
    Task UpdateOrderStatusAsync(int orderId, string status);
}
