using BLL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MNP_main.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MNP_main.Controllers
{
    [Authorize(Roles = "Admin,Worker")]
    public class FoodController : Controller
    {
        private readonly ILogger<FoodController> _logger;
        private readonly IFoodService _foodService;
        private readonly IMenuService _menuService;

        public FoodController(ILogger<FoodController> logger, IFoodService foodService, IMenuService menuService)
        {
            _logger = logger;
            _foodService = foodService;
            _menuService = menuService;
        }

        public async Task<IActionResult> AddFood()
        {
            var food = await _foodService.GetAllFoodAsync();
            return View(food.Select(x => new FoodViewModel(x)).ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFood([FromForm] FoodViewModel data)
        {
            await _foodService.AddFoodAsync(data.Name, data.Description, (Food.MealType)data.Meal, (Food.DishType)data.Dish);
            return await AddFood();
        }

        [AllowAnonymous]
        public async Task<IActionResult> SelectFood(int selectKey, int mealType, int dishType)
        {
            Menu.DaysInWeek day = Helpers.Helpers.GetCurrentDay();

            Menu menu = await _menuService.GetMenuByDay(day);

            List<FoodViewModel> foods = (await _foodService.GetAllFoodAsync())
                .Where(x => menu.Foods.Contains(x) && x.Meal == (Food.MealType)mealType && x.Type == (Food.DishType)dishType)
                .Select(x => new FoodViewModel(x))
                .ToList();

            return View("SelectFood", foods);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> RemoveFood(int foodId)
        {
            await _foodService.RemoveFood(foodId);
            var food = await _foodService.GetAllFoodAsync();

            return View("AddFood", food.Select(x => new FoodViewModel(x)).ToList());
        }
    }
}
