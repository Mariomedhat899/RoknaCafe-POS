using System.Threading.Tasks;
using Rokna.Domain.Entities;

namespace Rokna.Domain.Interfaces;

public interface ICategoryService 
{
  Task<IEnumerable<Category>> GetAllAsync();
  Task<IEnumerable<Category>> GetActiveAsync();
  Task<Category?> GetByIdAsync(int id);
  Task<Category> CreateAsync(string name, int displayOrder);
  Task UpdateAsync(int id, string name , bool IsActive, int displayOrder);
  Task DeleteAsync(int id);
  
}