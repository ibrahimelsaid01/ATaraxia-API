namespace ATaraxia.Core.Models;

public class Template
{
    public Template()
    {
        UserLikes = new HashSet<UserLike>();
    }
    public Guid TemplateId { get; set; }
    public TemplateType Type { get; set; }
    public string? Title { get; set; }
    public string? FileUrl { get; set; }
    public string? Picture { get; set; }
    public string? File { get; set; }
    public bool IsVideo { get; set; }   
    public ICollection<UserLike>? UserLikes { get; set; }
}


