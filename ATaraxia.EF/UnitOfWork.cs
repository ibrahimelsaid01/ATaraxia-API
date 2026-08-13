namespace ATaraxia.EF;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public ITemplateRepository Templates { get; private set; }
    public IUserRepository Users { get; private set; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;

        Templates = new TemplateRepository(_context);
        Users = new UserRepository(_context);
    }
    public void Dispose()
    {
        _context.Dispose();
    }
    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
