namespace mas.Models;

public class Testimonial
{
    public int Id { get; set; }
    public required string CustomerName { get; set; }
    public string? CustomerTitle { get; set; }
    public string? CustomerImage { get; set; }
    public required string TestimonialText { get; set; }
  public int Rating { get; set; } = 5; // 1-5 stars
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
