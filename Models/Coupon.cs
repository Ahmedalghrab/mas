using mas.Models.Base;

namespace mas.Models;

public class Coupon : BaseEntity, ISoftDelete
{
    public string Code { get; set; } = null!;
    public string DescriptionAr { get; set; } = null!;
    public string? DescriptionEn { get; set; }
    public CouponType Type { get; set; }
    public decimal Value { get; set; }
    public decimal? MinimumOrderAmount { get; set; }
    public decimal? MaximumDiscountAmount { get; set; }
    public int? MaxUsageCount { get; set; }
    public int CurrentUsageCount { get; set; } = 0;
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Soft Delete
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }
}

public enum CouponType
{
    Percentage,
    FixedAmount
}
