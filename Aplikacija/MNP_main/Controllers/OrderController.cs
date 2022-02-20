using BLL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MNP_main.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MNP_main.Controllers
{
    [Authorize(Roles = "Worker,Admin")]
    public class OrderController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        private readonly AspNetUserManager<User> _userManager;


        public OrderController(ILogger<UserController> logger, IUserService userService, IOrderService order, AspNetUserManager<User> userManager)
        {
            this._logger = logger;
            this._userService = userService;
            this._orderService = order;
            this._userManager = userManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> OrderHistory()
        {
            var korisnik = await _userManager.GetUserAsync(User);
            var orders = (await _userService.GetUserByIdAsync(korisnik.Id)).Orders.ToList();
            return View(orders.Select(o => new OrderViewModel(o)).ToList());
        }

        public async Task<IActionResult> OrderManagement()
        {
            List<OrderViewModel> viewOrders = new List<OrderViewModel>();
            List<Order> orders = await _orderService.GetAllOrdersAsync();
            orders.ForEach(o => {
                OrderViewModel ord = new OrderViewModel();
                ord.Id = o.Id.ToString();
                ord.SpecialRequest = o.SpecialRequest;
                ord.Price = o.Price.ToString();
                ord.OrderDate = o.OrderDate;
                ord.DeliveryDate = o.DeliveryDate;
                ord.Client = o.Client;
                ord.Deliverer = o.Deliverer;
                viewOrders.Add(ord);
            });
            return View("OrderManagement", viewOrders);
        }

        [AllowAnonymous]
        public async Task<IActionResult> OrderInformation()
        {
            ViewBag.deliveryGuys = (List<User>)(await _userManager.GetUsersInRoleAsync("DeliveryGuy"));

            int id = int.Parse(Request.RouteValues["Id"].ToString());
            Order o = await _orderService.GetOrderById(id);
            return View("OrderInformation", new OrderViewModel()
            {
                Id = o.Id.ToString(),
                SpecialRequest = o.SpecialRequest,
                Price = o.Price.ToString(),
                OrderDate = o.OrderDate,
                DeliveryDate = o.DeliveryDate,
                Client = o.Client,
                Breakfast = o.Breakfast,
                Lunch = o.Lunch,
                Dinner = o.Dinner
            });
        }

        [HttpPost]
        public async Task<IActionResult> OrderConfirmed(string Id, string IdOrder)
        {
            User deliverer = await _userService.GetUserByIdAsync(Id);
            Order order = await _orderService.GetOrderById(int.Parse(IdOrder));
            await _orderService.SetDeliveryGuyAsync(order.Id, deliverer.Id);
            return await OrderManagement();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}