using DAL.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class Order : IEntity
    {
        public int Id { get; set; }
        public string SpecialRequest { get; set; }
        public int Price { get; set; }
        public System.DateTime OrderDate { get; set; }
        public System.DateTime DeliveryDate { get; set; }
        public User Client { get; set; }
        public User Deliverer { get; set; }
        public bool Delivered { get; set; }

        public virtual ICollection<Food> Breakfast { get; set; }
        public virtual ICollection<Food> Lunch { get; set; }
        public virtual ICollection<Food> Dinner { get; set; }


        public Order()
        {
            Breakfast   = new HashSet<Food>();
            Lunch       = new HashSet<Food>();
            Dinner      = new HashSet<Food>();
        }
    }
}
