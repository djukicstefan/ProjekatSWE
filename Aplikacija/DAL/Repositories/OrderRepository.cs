using DAL.Interfaces;
using DAL.Models;

namespace DAL.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext ctx) : base(ctx) {}
    }
}
