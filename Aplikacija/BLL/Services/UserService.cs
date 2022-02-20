using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UserService : Service, IUserService
    {
        public UserService(IUnitOfWork uow) : base(uow) { }

        public Task DeleteUser(string id) => Users.DeleteAsync(id);

        public Task<List<User>> GetAllUsersAsync()
            => Users.GetAllAsync();

        public Task<User> GetUserByIdAsync(string id)
            => Users.GetByIdAsync(id, "Orders", "OrdersToDeliver.Client");

        public async Task UpdateUsersBalance(User user, double balance)
        {
            var u = Users.Update(user);
            u.Balance = balance;
            await SaveChangesAsync();
        }

        public async Task UpdateUsersAddress(User user, string addr)
        {
            var u = Users.Update(user);
            u.Address = addr;
            await SaveChangesAsync();
        }

        public async Task UpdateUsersPhoneNumber(User user, string phone)
        {
            var u = Users.Update(user);
            u.PhoneNumber = phone;
            await SaveChangesAsync();
        }
    }
}
