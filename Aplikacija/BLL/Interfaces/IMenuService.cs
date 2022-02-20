using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IMenuService
    {
        Task AddFoodAsync(int menuId, int foodId);
        Task AddFoodAsync(int menuId, Food food);
        Task AddMenuAsync(Menu.DaysInWeek day, ICollection<Food> food = null);
        Task DeleteMenu(int id);
        Task<List<Menu>> GetAllMenusAsync();
        Task<Menu> GetMenuByDay(Menu.DaysInWeek day);
        Task RemoveFoodAsync(int menuid, int foodId);
        Task RemoveFoodAsync(int menuId, Food food);
    }
}
