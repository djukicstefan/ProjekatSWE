using DAL.Models;
using BLL.Interfaces;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MNP_main.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MNP_main.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IUserService _userService;
        private readonly ICommentService _commentService;
        private readonly IOrderService _orderService;

        public UsersController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IUserService userService, ICommentService commentService, IOrderService orderService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this._userService = userService;
            this._commentService = commentService;
            this._orderService = orderService;
        }

        public IActionResult Index() => View(userManager.Users.ToList());
        public IActionResult Return() => View("Index", userManager.Users.ToList());

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromForm] UserViewModel user)
        {
            if (user.Password != user.ConfirmPassword)
                return View("Error", "Password and ConfirmPassword must match.");

            var newUser = new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                CardNumber = user.CardNumber,
                Email = user.Email,
                UserName = user.UserName,
                UMCN = user.UCMN,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(newUser, user.Password);

            if (await roleManager.RoleExistsAsync(user.Role))
                await userManager.AddToRoleAsync(newUser, user.Role);

            if (result.Succeeded)
                return View("Index", userManager.Users.ToList());

            return View("Error", result.Errors.Select(x => x.Description).ToArray());
        }

        public async Task<IActionResult> EditUser()
        {
            string Id = Request.RouteValues["Id"].ToString();

            User u = await _userService.GetUserByIdAsync(Id);

            return View("EditUser", u);
        }

        public async Task<IActionResult> DeleteUser()
        {
            string Id = Request.RouteValues["Id"].ToString();

            User u = await _userService.GetUserByIdAsync(Id);

            List<Comment> comments = await _commentService.GetAllCommentsFromUser(u);
            for (int i=0; i<comments.Count; ++i)
                await _commentService.RemoveComment(comments[i].Id);

            List<Order> orders = (await _orderService.GetAllOrdersAsync()).Where(x => x.Client.Id == u.Id).ToList();
            for (int i = 0; i < orders.Count; ++i)
                await _orderService.RemoveOrder(orders[i].Id);

            await userManager.DeleteAsync(u);

            return View("Index", userManager.Users.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUser([FromForm] UserViewModel user)
        {
            User u = await userManager.FindByIdAsync(user.Id);


            if (user.Password != null)
            {
                if (user.Password != user.ConfirmPassword)
                    return View("Error", new string[] { "Password and ConfirmPassword must match." });

                await userManager.RemovePasswordAsync(u);
                await userManager.AddPasswordAsync(u, user.Password);
            }

            if (user.FirstName != null)
            {
                u.FirstName = user.FirstName;
            }

            if (user.LastName != null)
            {
                u.LastName = user.LastName;
            }

            if (user.Address != null)
            {
                u.Address = user.Address;
            }

            if (user.PhoneNumber != null)
            {
                u.PhoneNumber = user.PhoneNumber;
            }

            if (user.CardNumber != null)
            {
                u.CardNumber = user.CardNumber;
            }

            if (user.Email != null)
            {
                u.Email = user.Email;
                u.EmailConfirmed = true;
            }

            if (user.UserName != null)
            {
                u.UserName = user.UserName;
            }

            if (user.UCMN != null)
            {
                u.UMCN = user.UCMN;
            }

            var result = await userManager.UpdateAsync(u);


            if (result.Succeeded)
                return View("Index", userManager.Users.ToList());

            return View("Error", result.Errors.Select(x => x.Description).ToArray());
        }
    }
}
