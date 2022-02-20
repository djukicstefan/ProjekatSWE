using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models;

namespace MNP_main.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UCMN { get; set; }
        public string CardNumber { get; set; }
        public string Address { get; set; }
        public string Balance { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public UserViewModel()
        {
           
        }

        public UserViewModel(User user)
        {
            Id = user.Id;
            UserName = user.UserName;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            UCMN = user.UMCN;
            CardNumber = user.CardNumber;
            Address = user.Address;
            Balance = user.Balance.ToString();
        }
    }
    
}
