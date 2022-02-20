using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class MenuService : Service, IMenuService
    {
        public MenuService(IUnitOfWork uow) : base(uow) {}

        public async Task AddMenuAsync(Menu.DaysInWeek day, ICollection<Food> food = null)
        {
            Menu menu = new() { 
                Day = day
            };

            if (food != null)
                foreach (var f in food)
                    menu.Foods.Add(f);

            await Menus.AddAsync(menu);
            await SaveChangesAsync();
        }

        public Task<List<Menu>> GetAllMenusAsync() => Menus.GetAllAsync("Foods");

        public async Task<Menu> GetMenuByDay(Menu.DaysInWeek day) 
            => Menus.Find(x => x.Day == day, "Foods").FirstOrDefault();

        public async Task DeleteMenu(int id)
        {
            await Food.DeleteAsync(id);
            await SaveChangesAsync();
        }

        public async Task AddFoodAsync(int menuId, int foodId)
            => await AddFoodAsync(menuId, await Food.GetByIdAsync(foodId));

        public async Task AddFoodAsync(int menuId, Food food)
        {
            var menu = await Menus.Update(menuId);
            menu.Foods.Add(food);
            await SaveChangesAsync();
        }

        public async Task RemoveFoodAsync(int menuid, int foodId)
            => await RemoveFoodAsync(menuid, await Food.GetByIdAsync(foodId));

        public async Task RemoveFoodAsync(int menuId, Food food)
        {
            var menu = await Menus.Update(menuId);
            menu.Foods.Remove(food);
            await SaveChangesAsync();
        }
    }
}
