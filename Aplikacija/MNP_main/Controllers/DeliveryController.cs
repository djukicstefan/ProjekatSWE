using BLL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MNP_main.Controllers
{
    [Authorize(Roles = "DeliveryGuy")]
    public class DeliveryController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        private readonly AspNetUserManager<User> _userManager;

        public DeliveryController(IOrderService orderService, IUserService userService, AspNetUserManager<User> userManager)
        {
            _orderService = orderService;
            _userService = userService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userService.GetUserByIdAsync(_userManager.GetUserId(User));
            return View("Index", user.OrdersToDeliver.Where(x => !x.Delivered).ToList());
        }
        
        public async Task<IActionResult> Deliver([FromRoute(Name = "id")] int id)
        {
            await _orderService.Deliver(id);
            return await Index();
        }
    }
}
