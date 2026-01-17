using Microsoft.AspNetCore.Identity;

namespace mas.Models;

public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Marketing & Customer preferences
    public bool AcceptsMarketing { get; set; } = true;
    public DateTime? LastLoginAt { get; set; }
}
