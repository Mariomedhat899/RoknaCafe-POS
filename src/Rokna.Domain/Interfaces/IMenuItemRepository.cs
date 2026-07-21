using Rokna.Domain.Entities;

namespace Rokna.Domain.Interfaces;

public interface IMenuItemRepository
{
  Task<IEnumerable<MenuItem>> GetByCategoryAsync(int categoryId, CancellationToken cancellationToken = default);
  Task<IEnumerable<MenuItem>> GetAvailableAsync(CancellationToken cancellationToken = default);
  Task<MenuItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
  Task AddAsync(MenuItem menuItem, CancellationToken cancellationToken = default);
  void Update(MenuItem menuItem);
  void Delete(MenuItem menuItem);
  Task SaveChangesAsync(CancellationToken cancellationToken = default);

}