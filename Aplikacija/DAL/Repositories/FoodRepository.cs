using DAL.Interfaces;
using DAL.Models;

namespace DAL.Repositories
{
    public class FoodRepository : Repository<Food>, IFoodRepository
    {
        public FoodRepository(ApplicationDbContext ctx) : base(ctx) { }
    }
}
