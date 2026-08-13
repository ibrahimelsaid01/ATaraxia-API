namespace ATaraxia.EF.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected ApplicationDbContext _context;

    public BaseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<T?> GetByIdAsync(Guid id, string[]? includes = null)
    {
        IQueryable<T> query = _context.Set<T>();

        if (includes != null)
            foreach (var include in includes)
                query = query.Include(include);

        var keyProperty = _context.Model.FindEntityType(typeof(T))!
            .FindPrimaryKey()!
            .Properties
            .Single();

        return await query.SingleOrDefaultAsync(
            e => Microsoft.EntityFrameworkCore.EF.Property<Guid>(e, keyProperty.Name) == id);
    }


    public async Task<IEnumerable<T>> GetAllAsync(string[]? includes = null)
    {
        IQueryable<T> query = _context.Set<T>();

        if (includes != null)
            foreach (var include in includes)
                query = query.Include(include);

        return await query.ToListAsync();
    }


    public async Task<T?> FindAsync(Expression<Func<T, bool>> criteria, string[]? includes = null)
    {
        IQueryable<T> query = _context.Set<T>();

        if (includes != null)
            foreach (var include in includes)
                query = query.Include(include);

        return await query.SingleOrDefaultAsync(criteria);
    }

    public async Task<T> CreateAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        return entity;
    }


    public async Task UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);

    }



    public async Task DeleteAsync(Guid id)
    {
        var template = await _context.Set<T>().FindAsync(id);

        if (template != null)
        {
            _context.Set<T>().Remove(template);
        }
    }
}