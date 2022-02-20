using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class OrderService : Service, IOrderService
    {
        public OrderService(IUnitOfWork uow) : base(uow) {}

        public async Task<bool> AddFoodAsync(int orderId, Food food)
        {
            var order = await Orders.Update(orderId);

            var (orderFoods, dishesAvaliable) = food.Meal switch
            {
                DAL.Models.Food.MealType.Breakfast => (order.Breakfast, DAL.Models.Food.BREAKFAST_DISHES),
                DAL.Models.Food.MealType.Lunch => (order.Lunch, DAL.Models.Food.LUNCH_DISHES),
                DAL.Models.Food.MealType.Dinner => (order.Dinner, DAL.Models.Food.DINNER_DISHES),
                _ => default
            };

            if (orderFoods == null) return false;

            if (dishesAvaliable.Count(x => food.Type == x) < orderFoods.Count(x => food.Type == x.Type)) 
                return false; // unable to add type of food to the selected order

            orderFoods.Add(food);
            await SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddFoodAsync(int orderId, int foodId)
        {
            var result = await AddFoodAsync(orderId, await Food.GetByIdAsync(foodId));
            await SaveChangesAsync();
            return result;
        }

        public async Task<Order> CreateNewAsync(Order order)
        {
            var o = await Orders.AddAsync(order);
            await SaveChangesAsync();
            return o;
        }

        public Task<List<Order>> GetAllOrdersAsync() => Orders.GetAllAsync("Client", "Deliverer", "Breakfast", "Lunch", "Dinner");

        public async Task<Order> GetOrderById(int id) => Orders.Find(x => x.Id == id, "Client", "Deliverer", "Breakfast", "Lunch", "Dinner").FirstOrDefault();

        public async Task RemoveFoodAsync(int orderId, Food food)
        {
            var order = await Orders.Update(orderId);
            (food.Meal switch
            {
                DAL.Models.Food.MealType.Breakfast => order.Breakfast,
                DAL.Models.Food.MealType.Lunch => order.Lunch,
                DAL.Models.Food.MealType.Dinner => order.Dinner,
                _ => null
            })?.Remove(food);
            await SaveChangesAsync();
        }

        public async Task RemoveFoodAsync(int orderId, int foodId) 
            => await RemoveFoodAsync(orderId, await Food.GetByIdAsync(foodId));

        public Task RemoveOrder(int orderId) => Orders.DeleteAsync(orderId);

        public async Task SetDeliveryGuyAsync(int orderId, string guyId)
        {
            var order = await Orders.Update(orderId);
            order.Deliverer = await Users.GetByIdAsync(guyId);
            await SaveChangesAsync();
        }

        public async Task Update(int orderId, Order order)
        {
            var o = await Orders.Update(orderId);
            o.Breakfast = order.Breakfast;
            o.Lunch = order.Lunch;
            o.Dinner = order.Dinner;
            o.Client = order.Client;
            o.Deliverer = order.Deliverer;
            o.DeliveryDate = order.DeliveryDate;
            o.OrderDate = order.OrderDate;
            o.Price = order.Price;
            o.SpecialRequest = order.SpecialRequest;
            o.Delivered = order.Delivered;
            await SaveChangesAsync();
        }

        public async Task Deliver(int orderId)
        {
            var order = await GetOrderById(orderId);
            order.DeliveryDate = DateTime.Now;
            order.Delivered = true;
            await Update(orderId, order);
        }
    }
}
