namespace ATaraxia.Core;
public interface IUnitOfWork : IDisposable
{
   ITemplateRepository Templates { get; }
   IUserRepository Users { get; }

    Task<int> CompleteAsync();
}
