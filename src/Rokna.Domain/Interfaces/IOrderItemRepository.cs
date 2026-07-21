using Rokna.Domain.Entities;
namespace Rokna.Domain.Interfaces;
public interface IOrderItemRepository
{
  Task<IEnumerable<OrderItem>> GetByOrderIdAsync(int orderId, CancellationToken cancellationToken = default);
  Task AddAsync(OrderItem orderItem,CancellationToken cancellationToken = default);
  Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
