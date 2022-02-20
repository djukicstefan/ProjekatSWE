using DAL.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class Food : IEntity
    {
        public enum DishType
        {
            Main,
            Soup,
            Salad,
            Desert
        }

        public enum MealType
        {
            Breakfast,
            Lunch,
            Dinner
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsAvaliable { get; set; }
        public MealType Meal { get; set; }
        public DishType Type { get; set; }
        public virtual ICollection<Menu> Menu { get; set; }

        [InverseProperty("Breakfast")]
        public virtual ICollection<Order> BreakfastOrders { get; set; }
        [InverseProperty("Lunch")]
        public virtual ICollection<Order> LunchOrders { get; set; }
        [InverseProperty("Dinner")]
        public virtual ICollection<Order> DinnerOrders { get; set; }


        public static readonly DishType[] BREAKFAST_DISHES = { DishType.Main, DishType.Desert };
        public static readonly DishType[] LUNCH_DISHES= { DishType.Soup, DishType.Main, DishType.Salad, DishType.Desert };
        public static readonly DishType[] DINNER_DISHES = { DishType.Main, DishType.Salad, DishType.Desert };

        public Food()
        {
            Menu = new HashSet<Menu>();
            BreakfastOrders = new HashSet<Order>();
            LunchOrders = new HashSet<Order>();
            DinnerOrders = new HashSet<Order>();
        }
    }
}
