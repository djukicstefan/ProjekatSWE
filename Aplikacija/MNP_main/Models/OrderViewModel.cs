using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models;

namespace MNP_main.Models
{
    public class OrderViewModel
    {
        public string Id { get; set; }
        public string SpecialRequest { get; set; }
        public string Price { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public User Client { get; set; }
        public User Deliverer { get; set; }
        public bool Delivered { get; set; }

        public virtual ICollection<Food> Breakfast { get; set; }
        public virtual ICollection<Food> Lunch { get; set; }
        public virtual ICollection<Food> Dinner { get; set; }

        public OrderViewModel()
        {

        }

        public OrderViewModel(Order order)
        {
            Id = order.Id.ToString();
            SpecialRequest = order.SpecialRequest;
            Price = order.Price.ToString();
            OrderDate = order.OrderDate;
            DeliveryDate = order.DeliveryDate;
            Deliverer = order.Deliverer;
            Breakfast = order.Breakfast;
            Lunch = order.Lunch;
            Dinner = order.Dinner;
            Delivered = order.Delivered;
        }
    }

}
