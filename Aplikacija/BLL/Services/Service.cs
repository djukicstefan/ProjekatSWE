using BLL.Interfaces;
using DAL;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class Service : IService
    {
        protected IFoodRepository Food => UnitOfWork.FoodRepository;
        protected IMenuRepository Menus => UnitOfWork.MenuRepository;
        protected IOrderRepository Orders => UnitOfWork.OrderRepository;
        protected IUserRepository Users => UnitOfWork.UserRepository;
        protected ICommentRepository Comments => UnitOfWork.CommentRepository;

        public IUnitOfWork UnitOfWork { get; }
        public Service(IUnitOfWork uow) => UnitOfWork = uow;
        public Task SaveChangesAsync() => UnitOfWork.CommitChangesAsync();
    }
}
