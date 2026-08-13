namespace ATaraxia.Core.Models;

public class Question
{
    public Guid QuestionId { get; set; }

    public string? Ask { get; set; }
    public string? Answer { get; set; }

    public Guid UserId { get; set; }
    public virtual User? Users { get; set; }

}
