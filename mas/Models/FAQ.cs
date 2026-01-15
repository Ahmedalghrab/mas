namespace mas.Models;

public class FAQ
{
    public int Id { get; set; }
    public required string QuestionAr { get; set; }
    public required string QuestionEn { get; set; }
    public required string AnswerAr { get; set; }
    public required string AnswerEn { get; set; }
    public int DisplayOrder { get; set; } = 0;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
