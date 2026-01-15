using mas.Models.Base;

namespace mas.Models;

public class Review : BaseEntity, ISoftDelete
{
    public int ProductId { get; set; }
    public string UserId { get; set; } = null!;
    public int Rating { get; set; } // 1-5
    public string? Comment { get; set; }
    public bool IsVerified { get; set; } = false;
    public bool IsApproved { get; set; } = false;
    
    // Soft Delete
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }

    // Navigation Properties
    public Product Product { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
}
