using DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(string id);
        Task<List<User>> GetAllUsersAsync();
        Task DeleteUser(string id);
        Task UpdateUsersBalance(User user, double balance);
        Task UpdateUsersAddress(User user, string addr);
        Task UpdateUsersPhoneNumber(User user, string phone);
    }
}
