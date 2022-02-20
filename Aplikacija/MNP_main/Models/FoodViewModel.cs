using DAL.Models;

namespace MNP_main.Models
{
    public class FoodViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public MealType Meal { get; set; }
        public DishType Dish { get; set; }

        public FoodViewModel() { }

        public FoodViewModel(Food food)
        {
            Id = food.Id;
            Name = food.Name;
            Description = food.Description;
            Meal = (MealType)food.Meal;
            Dish = (DishType)food.Type;
        }

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

    }
}
