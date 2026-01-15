using mas.Models.Base;

namespace mas.Models;

public class Notification : BaseEntity
{
    public string UserId { get; set; } = null!;
    public string TitleAr { get; set; } = null!;
    public string? TitleEn { get; set; }
    public string MessageAr { get; set; } = null!;
    public string? MessageEn { get; set; }
    public NotificationType Type { get; set; }
    public bool IsRead { get; set; } = false;
    public DateTime? ReadAt { get; set; }
    public string? Link { get; set; }

    // Navigation Properties
    public ApplicationUser User { get; set; } = null!;
}

public enum NotificationType
{
    System,
    Order,
    Product,
    Message,
    Promotion
}
