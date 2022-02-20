using DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class User : IdentityUser, IEntity<string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UMCN { get; set; } // JMBG
        public string CardNumber { get; set; }
        public string Address { get; set; }
        public double Balance { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Order> OrdersToDeliver { get; set; }

        public User()
        {
            Orders = new HashSet<Order>();
        }
    }
}