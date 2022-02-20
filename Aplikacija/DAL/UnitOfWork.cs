using DAL.Interfaces;
using System.Threading.Tasks;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        public IOrderRepository OrderRepository { get; }
        public IFoodRepository FoodRepository { get; }
        public IMenuRepository MenuRepository { get; }
        public IUserRepository UserRepository { get; }
        public ICommentRepository CommentRepository { get; }

        public UnitOfWork(IOrderRepository orderRepo, IFoodRepository foodRepo, IMenuRepository menuRepo, IUserRepository userRepo, ICommentRepository commentRepo)
        {
            OrderRepository = orderRepo;
            FoodRepository = foodRepo;
            MenuRepository = menuRepo;
            UserRepository = userRepo;
            CommentRepository = commentRepo;
        }

        public async Task CommitChangesAsync()
        {
            await OrderRepository.SaveAsync();
            await FoodRepository.SaveAsync();
            await MenuRepository.SaveAsync();
            await UserRepository.SaveAsync();
            await CommentRepository.SaveAsync();
        }
    }
}
