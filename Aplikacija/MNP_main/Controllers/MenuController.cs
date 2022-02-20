using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BLL.Interfaces;
using System.Linq;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;

namespace MNP_main.Controllers
{
    [Authorize(Roles = "Worker,Admin")]
    public class MenuController : Controller
    {
        private readonly ILogger<MenuController> _logger;
        private readonly IMenuService _menuService;
        private readonly IFoodService _foodService;

        public MenuController(ILogger<MenuController> logger, IMenuService menuService, IFoodService foodService)
        {
            _logger = logger;
            _menuService = menuService;
            _foodService = foodService;
        }

        public async Task<IActionResult> EditMenu()
        {
            var menus = await _menuService.GetAllMenusAsync();
            ViewBag.foods = await _foodService.GetAllFoodAsync();
        
            return View("EditMenu", menus.ToList());
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFood(int dayIndex, int foodID)
        { 
            if (foodID >= 0)
            { 
                Menu menu = await _menuService.GetMenuByDay((Menu.DaysInWeek)dayIndex);
                await _menuService.AddFoodAsync(menu.Id, foodID);
            }

            return Redirect(Url.Action("EditMenu", "Menu", new { day = dayIndex }));
        }

        public async Task<IActionResult> RemoveFood(int dayIndex, int foodId)
        {
            Menu menu = await _menuService.GetMenuByDay((Menu.DaysInWeek)dayIndex);
            if (menu != null)
                await _menuService.RemoveFoodAsync(menu.Id, foodId);

            return Redirect(Url.Action("EditMenu", "Menu", new { day = dayIndex }));
        }
    }
}
