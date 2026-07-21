using System.Collections.Generic;
using System.Threading.Tasks;
using Rokna.Domain.Entities;

namespace Rokna.Domain.Interfaces;

public interface IMenuItemService
{
    Task<IEnumerable<MenuItem>> GetByCategoryAsync(int categoryId);
    Task<IEnumerable<MenuItem>> GetAvailableAsync();
    Task<MenuItem?> GetByIdAsync(int id);
    Task<MenuItem> CreateAsync(string name, decimal price, int categoryId);
    Task UpdateAsync(int id, string name, decimal price, bool isAvailable, int categoryId);
    Task DeleteAsync(int id);
}
