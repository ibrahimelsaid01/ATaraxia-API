namespace ATaraxia.Core.Repositories;

public interface IBaseRepository<T> where T : class
{

    Task<T?> GetByIdAsync(Guid id, string[]? includes = null);
    Task<IEnumerable<T>> GetAllAsync(string[]? includes = null );

    Task<T?> FindAsync(Expression<Func<T, bool>> criteria, string[]? includes = null);


    Task<T> CreateAsync(T entity);

    Task UpdateAsync(T entity);

    Task DeleteAsync(Guid id);



}
