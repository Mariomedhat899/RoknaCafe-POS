using Rokna.Domain.Entities;
namespace Rokna.Domain.Interfaces;
public interface IOrderRepository
{
  Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken = default);
  Task<IEnumerable<Order>> GetByDateRangeAsync(DateTime start, DateTime end, CancellationToken cancellationToken = default);
  Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
  Task AddAsync(Order order, CancellationToken cancellationToken = default);
  void Update(Order order);
  void Delete(Order order);
  Task SaveChangesAsync(CancellationToken cancellationToken = default);
}