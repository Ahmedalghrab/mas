using AutoMapper;
using mas.DTOs;
using mas.Models;
using mas.Repositories;

namespace mas.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<OrderService> _logger;

    public OrderService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<OrderService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<OrderDto> CreateOrderAsync(string userId, CreateOrderDto dto)
    {
        await _unitOfWork.BeginTransactionAsync();
        
        try
        {
            var order = new Order
            {
                OrderNumber = GenerateOrderNumber(),
                UserId = userId,
                Status = OrderStatus.Pending,
                PaymentStatus = PaymentStatus.Pending,
                Notes = dto.Notes,
                CouponCode = dto.CouponCode,
                CreatedAt = DateTime.UtcNow
            };

            decimal totalAmount = 0;
            decimal discountAmount = 0;

            foreach (var item in dto.Items)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
                if (product == null)
                    throw new KeyNotFoundException($"Product {item.ProductId} not found");

                var price = product.DiscountPrice ?? product.Price;
                var itemTotal = price * item.Quantity;
                totalAmount += itemTotal;

                var orderItem = new OrderItem
                {
                    ProductId = product.Id,
                    ProductNameAr = product.NameAr,
                    ProductNameEn = product.NameEn,
                    Quantity = item.Quantity,
                    UnitPrice = price,
                    TotalPrice = itemTotal,
                    DiscountAmount = product.DiscountPrice.HasValue ? 
                        (product.Price - product.DiscountPrice.Value) * item.Quantity : 0
                };

                order.OrderItems.Add(orderItem);
                discountAmount += orderItem.DiscountAmount;
            }

            // Apply coupon if provided
            if (!string.IsNullOrEmpty(dto.CouponCode))
            {
                var coupon = await _unitOfWork.Coupons.FirstOrDefaultAsync(c => 
                    c.Code == dto.CouponCode && 
                    c.IsActive && 
                    !c.IsDeleted &&
                    c.ValidFrom <= DateTime.UtcNow &&
                    c.ValidTo >= DateTime.UtcNow);

                if (coupon != null)
                {
                    var couponDiscount = coupon.Type == CouponType.Percentage
                        ? totalAmount * (coupon.Value / 100)
                        : coupon.Value;

                    if (coupon.MaximumDiscountAmount.HasValue)
                        couponDiscount = Math.Min(couponDiscount, coupon.MaximumDiscountAmount.Value);

                    discountAmount += couponDiscount;
                    coupon.CurrentUsageCount++;
                    _unitOfWork.Coupons.Update(coupon);
                }
            }

            order.TotalAmount = totalAmount;
            order.DiscountAmount = discountAmount;
            order.FinalAmount = totalAmount - discountAmount;

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            _logger.LogInformation("Order created: {OrderNumber} for user {UserId}", order.OrderNumber, userId);

            return _mapper.Map<OrderDto>(order);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            _logger.LogError(ex, "Error creating order for user {UserId}", userId);
            throw;
        }
    }

    public async Task<OrderDto?> GetOrderByIdAsync(int id)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(id);
        return order != null ? _mapper.Map<OrderDto>(order) : null;
    }

    public async Task<IEnumerable<OrderDto>> GetUserOrdersAsync(string userId)
    {
        var orders = await _unitOfWork.Orders.FindAsync(o => o.UserId == userId);
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }

    public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
    {
        var orders = await _unitOfWork.Orders.GetAllAsync();
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }

    public async Task UpdateOrderStatusAsync(int orderId, string status)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
        if (order == null)
            throw new KeyNotFoundException($"Order {orderId} not found");

        if (Enum.TryParse<OrderStatus>(status, out var orderStatus))
        {
            order.Status = orderStatus;
            order.UpdatedAt = DateTime.UtcNow;
            
            _unitOfWork.Orders.Update(order);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Order status updated: {OrderNumber} to {Status}", order.OrderNumber, status);
        }
    }

    private string GenerateOrderNumber()
    {
        return $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
    }
}
