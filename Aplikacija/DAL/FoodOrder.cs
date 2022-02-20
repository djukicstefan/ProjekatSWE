using DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{
    public class FoodOrder
    {
        public int FoodId { get; set; }
        [ForeignKey(nameof(FoodId))]
        public Food Food { get; set; }

        public int OrderId { get; set; }
        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; }
    }
}
