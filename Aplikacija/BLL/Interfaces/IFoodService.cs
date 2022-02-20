using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IFoodService
    {
        Task AddFoodAsync(string name, string description, Food.MealType mealType, Food.DishType dishType);
        Task<List<Food>> GetAllFoodAsync();
        Task RemoveFood(int id);
        Task<Food> GetFoodById(int id);
        Task UpdateFood(int id, Food food);
    }
}
