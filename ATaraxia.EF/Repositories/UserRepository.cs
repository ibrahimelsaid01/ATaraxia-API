namespace ATaraxia.EF.Repositories;

public class UserRepository :BaseRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }
}
