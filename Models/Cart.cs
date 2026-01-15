using mas.Models.Base;

namespace mas.Models;

public class Cart : BaseEntity
{
    public string UserId { get; set; } = null!;
    
    // Navigation Properties
    public ApplicationUser User { get; set; } = null!;
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}
