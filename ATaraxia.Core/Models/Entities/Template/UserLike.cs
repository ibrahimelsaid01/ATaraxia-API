namespace ATaraxia.Core.Models;

public class UserLike
{
    public Guid UserlikeId { get; set; }
    public Guid TemplateId { get; set; }
    [JsonIgnore]
    public virtual Template? Template { get; set; }
}
