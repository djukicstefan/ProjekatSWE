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
using MNP_main.Helpers;
using static DAL.Models.Food;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;

namespace MNP_main.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFoodService _foodService;
        private readonly IMenuService _menuService;
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        private readonly ICommentService _commentService;
        private readonly AspNetUserManager<User> _userManager;

        public HomeController(ILogger<HomeController> logger, IFoodService foodService, IMenuService menuService, IOrderService orderService, IUserService userService, ICommentService commentService, AspNetUserManager<User> userManager)
        {
            _logger = logger;
            _foodService = foodService;
            _menuService = menuService;
            _orderService = orderService;
            _userService = userService;
            _commentService = commentService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            Menu.DaysInWeek day = Helpers.Helpers.GetCurrentDay();

            List<Menu> menus = await _menuService.GetAllMenusAsync();
            Menu menu = menus.Find(x => x.Day == day) ?? null;

            return View(menu);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string message = null)
        {
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, Message = message });
        }

        public async Task<IActionResult> SelectFood(int selectedFoodId)
        {
            Menu.DaysInWeek day = Helpers.Helpers.GetCurrentDay();

            List<Menu> menus = await _menuService.GetAllMenusAsync();
            Menu menu = menus.Find(x => x.Day == day) ?? null;

            var food = await _foodService.GetFoodById(selectedFoodId);

            var selected = HttpContext.Session.GetString("selectedFood") ?? "";
            var selectedIds = selected.Split(',').Where(x => int.TryParse(x, out _)).Select(x => int.Parse(x)).ToList();

            var foods = (await _foodService.GetAllFoodAsync()).Where(x => selectedIds.Contains(x.Id)).ToList();

            var indexOfSelectedType = foods.FindIndex(x => x.Meal == food.Meal && x.Type == food.Type);

            if (indexOfSelectedType >= 0)
                selectedIds[indexOfSelectedType] = selectedFoodId;
            else
                selectedIds.Add(selectedFoodId);

            HttpContext.Session.SetString("selectedFood", $"{string.Join(',', selectedIds)}");

            return View("Index", menu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MakeOrder(string specialRequest)
        {
            List<Food> foods = await _foodService.GetAllFoodAsync();

            var selectedFood = HttpContext.Session.GetString("selectedFood");

            Request.Cookies.TryGetValue(CookieRequestCultureProvider.DefaultCookieName, out string lang);
            
            string errMsg = lang == "c=sr-Latn-RS|uic=sr-Latn-RS" ?
                "Ništa nije odabrano" :
                "Nothing was selected";

            if (string.IsNullOrEmpty(selectedFood)) 
                return Error(errMsg);
            

            var selectedIds = selectedFood.Split(',').Where(x => int.TryParse(x, out _)).Select(x => int.Parse(x));

            var food = foods.Where(x => selectedIds.Contains(x.Id));

            Food GetByType(MealType mealType, DishType dishType)
                => food.FirstOrDefault(x => x.Meal == mealType && x.Type == dishType);

            Food breakfastMain = GetByType(MealType.Breakfast, DishType.Main);
            Food breakfastDesert = GetByType(MealType.Breakfast, DishType.Desert);
            bool hasBreakfast = breakfastMain != null || breakfastDesert != null;

            Food lunchMain = GetByType(MealType.Lunch, DishType.Main);
            Food lunchSalad = GetByType(MealType.Lunch, DishType.Salad);
            Food lunchSoup = GetByType(MealType.Lunch, DishType.Soup);
            Food lunchDesert = GetByType(MealType.Lunch, DishType.Desert);
            bool hasLunch = lunchMain != null || lunchSalad != null || lunchSoup != null || lunchDesert != null;

            Food dinnerMain = GetByType(MealType.Dinner, DishType.Main);
            Food dinnerSalad = GetByType(MealType.Dinner, DishType.Salad);
            Food dinnerDesert = GetByType(MealType.Dinner, DishType.Desert);
            bool hasDinner = dinnerMain != null || dinnerSalad != null || dinnerDesert != null;

            var user = await _userManager.GetUserAsync(User);

            Order order = new()
            {
                Client = user,
                OrderDate = DateTime.Now,
                DeliveryDate = DateTime.Now.AddDays(1),
                Price = Helpers.Helpers.CalculatePrice(hasBreakfast, hasLunch, hasDinner),
                SpecialRequest = specialRequest
            };

            errMsg = lang == "c=sr-Latn-RS|uic=sr-Latn-RS" ?
                "Dostigli ste maksimalan broj narudžbina za danas (2/2)" :
                "You reached maximum order count for today. (2/2)";

            if ((await _orderService.GetAllOrdersAsync()).Count(x => x.Client == user && x.OrderDate.Date == DateTime.Today) > 2)
                return Error(errMsg);

            errMsg = lang == "c=sr-Latn-RS|uic=sr-Latn-RS" ?
                $"Nemate dovoljno novca.\nVaše trenutno stanje je: {user.Balance}. Cena: {order.Price}<br>Novac možete uplatiti <a href=\"../User/Profile\">ovde</a>." :
                $"You don't have enough money.\nYour current balance is: {user.Balance}. Price: {order.Price}<br>You can deposit <a href=\"../User/Profile\">here</a>.";

            if (order.Price > user.Balance)
                return Error(errMsg);

            await _userService.UpdateUsersBalance(user, user.Balance - order.Price);

            var o = await _orderService.CreateNewAsync(order);
            var result = selectedIds.All(x => _orderService.AddFoodAsync(o.Id, x).Result);

            Menu.DaysInWeek day = Helpers.Helpers.GetCurrentDay();
            List<Menu> menus = await _menuService.GetAllMenusAsync();
            Menu menu = menus.Find(x => x.Day == day) ?? null;

            errMsg = lang == "c=sr-Latn-RS|uic=sr-Latn-RS" ?
                "Došlo je do greške prilikom dodavanja hrane u porudžbini. Proverite dostupnost izabrane hrane i pokušajte ponovo :)" :
                "There was an error when adding food to the order. Check it's avaliablity and try again :)";

            if (result)
            {
                HttpContext.Session.SetString("selectedFood", "");
                return Redirect(Url.Action("OrderInformation", "Order", new { o.Id, OrderSuccess = true }));
            }
            return Error(errMsg);
        }

        public async Task<IActionResult> ShowComments()
        {
            var comments = await _commentService.GetAllCommentsAsync();
            return View("Comments", comments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateComment(string commentText)
        {
            await _commentService.CreateNewAsync(new Comment()
            {
                Content = commentText,
                User = await _userManager.GetUserAsync(User),
                CreatedAt = DateTime.Now
            });

            var comments = await _commentService.GetAllCommentsAsync();
            return View("Comments", comments);
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new Microsoft.AspNetCore.Http.CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        public async Task<IActionResult> DeleteComment()
        {
            int Id = int.Parse(Request.RouteValues["Id"].ToString());

            Comment com = await _commentService.GetCommentById(Id);

            if(com != null)
                await _commentService.RemoveComment(com.Id);

            var comments = await _commentService.GetAllCommentsAsync();
            return View("Comments", comments);
        }

        public async Task<IActionResult> ResetSelected()
        {
            HttpContext.Session.SetString("selectedFood", "");

            Menu.DaysInWeek day = Helpers.Helpers.GetCurrentDay();
            List<Menu> menus = await _menuService.GetAllMenusAsync();
            Menu menu = menus.Find(x => x.Day == day) ?? null;

            return View("Index", menu);
        }
    }
}
