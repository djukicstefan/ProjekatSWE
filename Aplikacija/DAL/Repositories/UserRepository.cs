using DAL.Interfaces;
using DAL.Models;

namespace DAL.Repositories
{
    public class UserRepository : Repository<User, string>, IUserRepository
    {
        public UserRepository(ApplicationDbContext ctx) : base(ctx){ }
    }
}
