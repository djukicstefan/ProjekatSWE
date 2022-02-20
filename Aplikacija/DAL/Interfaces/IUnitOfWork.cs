using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUnitOfWork
    {
        public IOrderRepository OrderRepository { get; }
        public IFoodRepository FoodRepository { get; }
        public IMenuRepository MenuRepository { get; }
        public IUserRepository UserRepository { get; }
        public ICommentRepository CommentRepository { get; }

        public Task CommitChangesAsync();
    }
}
