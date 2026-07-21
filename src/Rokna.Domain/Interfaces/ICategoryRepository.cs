using System.ComponentModel.DataAnnotations;
using Rokna.Domain.Entities;

namespace Rokna.Domain.Interfaces;

public interface ICategoryRepository
{
  Task<IEnumerable<Category>> GetAllAsync(CancellationToken  cancellationToken = default);
  Task<IEnumerable<Category>> GetActiveAsync(CancellationToken cancellationToken = default);
  Task<Category?> GetByIdAsync(int id,CancellationToken cancellationToken = default);
  Task AddAsync(Category category, CancellationToken cancellationToken = default);
  void Update(Category category);
  void Delete(Category category);
  Task SaveChangesAsync(CancellationToken cancellationToken = default);
}