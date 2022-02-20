using DAL.Interfaces;
using DAL.Models;

namespace DAL.Repositories
{
    public class MenuRepository : Repository<Menu>, IMenuRepository
    {
        public MenuRepository(ApplicationDbContext ctx) : base(ctx) { }
    }
}
