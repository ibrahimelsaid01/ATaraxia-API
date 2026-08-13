namespace ATaraxia.EF.Repositories;

public class TemplateRepository : BaseRepository<Template>, ITemplateRepository
{
    public TemplateRepository(ApplicationDbContext context) : base(context)
    {
    }
}
