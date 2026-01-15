namespace mas.Models;

public class ContactMessage
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public string? Phone { get; set; }
    public required string Subject { get; set; }
    public required string Message { get; set; }
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class Customer
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public string? Phone { get; set; }
    public string? WhatsAppNumber { get; set; }
    public bool IsActive { get; set; } = true;
    public bool SubscribedToMarketing { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class MarketingCampaign
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string MessageContent { get; set; }
    public CampaignType Type { get; set; } // Email, WhatsApp, Both
    public int SentCount { get; set; } = 0;
    public DateTime? SentAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; }
}

public enum CampaignType
{
 Email = 1,
    WhatsApp = 2,
    Both = 3
}
