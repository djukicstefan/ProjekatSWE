using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class FoodService : Service, IFoodService
    {
        public FoodService(IUnitOfWork uow) : base(uow) {}

        public async Task AddFoodAsync(string name, string description, Food.MealType mealType, Food.DishType dishType)
        {
            await Food.AddAsync(new Food
            {
                Description = description,
                Name = name,
                IsAvaliable = true,
                Meal = mealType,
                Type = dishType
            });
            await SaveChangesAsync();
        }

        public Task<List<Food>> GetAllFoodAsync() => Food.GetAllAsync();

        public async Task RemoveFood(int id)
        {
            await Food.DeleteAsync(id);
            await SaveChangesAsync();
        }

        public async Task UpdateFood(int id, Food food)
        {
            var f = await Food.Update(id);
            if (f == null) return;
            f.Name        = food.Name;
            f.Description = food.Description;
            f.Type        = food.Type;
            f.Meal        = food.Meal;
            await SaveChangesAsync();
        }

        public Task<Food> GetFoodById(int id) => Food.GetByIdAsync(id);
    }
}
