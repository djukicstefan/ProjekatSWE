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
using System.Security.Claims;
using System.Threading.Tasks;

namespace MNP_main.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly AspNetUserManager<User> _userManager;

        public UserController(ILogger<UserController> logger, IUserService userService, AspNetUserManager<User> userManager)
        {
            this._logger = logger;
            this._userService = userService;
            this._userManager = userManager;
        }

        public async Task<IActionResult> Profile()
        {
            var korisnickoIme = HttpContext.User.Identity.Name;
            var users = await _userService.GetAllUsersAsync();
            var korisnik = users.Find(x => x.UserName == korisnickoIme);
            return View(new UserViewModel()
            {
                FirstName = korisnik.FirstName,
                LastName = korisnik.LastName,
                Email = korisnik.Email,
                PhoneNumber = korisnik.PhoneNumber,
                UCMN = korisnik.UMCN,
                CardNumber = korisnik.CardNumber,
                Address = korisnik.Address,
                Balance = korisnik.Balance.ToString()
            });
        }

        [HttpPost]
        public async Task<IActionResult> BalanceChange(string firstName, string lastName, string balance)
        {
            var korisnik = await _userManager.GetUserAsync(User);
            if (korisnik.FirstName == firstName && korisnik.LastName == lastName)
            {
                await _userService.UpdateUsersBalance(korisnik, int.Parse(balance));
            }
            return View("Profile", new UserViewModel()
            {
                FirstName = korisnik.FirstName,
                LastName = korisnik.LastName,
                Email = korisnik.Email,
                PhoneNumber = korisnik.PhoneNumber,
                UCMN = korisnik.UMCN,
                CardNumber = korisnik.CardNumber,
                Address = korisnik.Address,
                Balance = korisnik.Balance.ToString()
            });

        }

        [HttpPost]
        public async Task<IActionResult> PhoneNumberChange(string phoneNumber)
        {
            var korisnik = await _userManager.GetUserAsync(User);
            await _userService.UpdateUsersPhoneNumber(korisnik, phoneNumber);

            return View("Profile", new UserViewModel()
            {
                FirstName = korisnik.FirstName,
                LastName = korisnik.LastName,
                Email = korisnik.Email,
                PhoneNumber = korisnik.PhoneNumber,
                UCMN = korisnik.UMCN,
                CardNumber = korisnik.CardNumber,
                Address = korisnik.Address,
                Balance = korisnik.Balance.ToString()
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddressChange(string address)
        {
            var korisnik = await _userManager.GetUserAsync(User);
            await _userService.UpdateUsersAddress(korisnik, address);

            return View("Profile", new UserViewModel()
            {
                FirstName = korisnik.FirstName,
                LastName = korisnik.LastName,
                Email = korisnik.Email,
                PhoneNumber = korisnik.PhoneNumber,
                UCMN = korisnik.UMCN,
                CardNumber = korisnik.CardNumber,
                Address = korisnik.Address,
                Balance = korisnik.Balance.ToString()
            });
        }
        


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

}