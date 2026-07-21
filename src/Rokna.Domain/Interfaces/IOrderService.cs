using System.Threading.Tasks;
using Rokna.Domain.Entities;

namespace Rokna.Domain.Interfaces;

public interface IOrderService
{
  Task<IEnumerable<Order>> GetAllAsync();
  Task<IEnumerable<Order>> GetByDateRangeAsync(DateTime start, DateTime end);
  Task<Order?> GetByIdAsync(int id);
    Task<Order> CreateOrderAsync(string? orderNumber, string? notes, List<OrderItemRequest> items);
    Task<Order> CloseOrderAsync(int orderId, bool isPaid = true);
  
}

public class OrderItemRequest
{
    public int MenuItemId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
