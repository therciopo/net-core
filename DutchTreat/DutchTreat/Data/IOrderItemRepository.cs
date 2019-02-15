using DutchTreat.Data.Entities;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public interface IOrderItemRepository
    {
        Task<OrderItem> GetOrderItemById(int id);
    }
}