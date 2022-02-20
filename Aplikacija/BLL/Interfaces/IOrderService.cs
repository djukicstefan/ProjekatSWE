using DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IOrderService
    {
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderById(int id);
        Task<Order> CreateNewAsync(Order order);
        Task Update(int orderId, Order order);
        Task SetDeliveryGuyAsync(int orderId, string guyId);

        Task<bool> AddFoodAsync(int orderId, Food food);
        Task<bool> AddFoodAsync(int orderId, int foodId);
        Task RemoveFoodAsync(int orderId, Food food);
        Task RemoveFoodAsync(int orderId, int foodId);
        Task Deliver(int orderId);

        Task RemoveOrder(int orderId);
    }
}
