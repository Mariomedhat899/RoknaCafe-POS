using Rokna.Domain.Entities;
using Rokna.Domain.Interfaces;
using Rokna.Infrastructure.Repositories;

namespace Rokna.Infrastructure.Services;

public class CategoryService : ICategoryService
{
  private readonly ICategoryRepository _categoryRepository;

  public CategoryService(ICategoryRepository categoryRepository)
  {
   _categoryRepository = categoryRepository; 
  }

  public async Task<IEnumerable<Category>> GetAllAsync()
  {
   return await _categoryRepository.GetAllAsync();
  }

  public async Task<IEnumerable<Category>> GetActiveAsync()
  {
    return await _categoryRepository.GetActiveAsync();
  }

  public async Task<Category?> GetByIdAsync(int id) 
    {
     return await _categoryRepository.GetByIdAsync(id);
    }

  public async Task<Category> CreateAsync(string name, int displayOrder)
  {
   var category = new Category
   {
    Name = name,
    DisplayOrder = displayOrder,
    IsActive = true
   };
   await _categoryRepository.AddAsync(category);
   await  _categoryRepository.SaveChangesAsync();

   return category;
  }

  public async Task UpdateAsync(int id, string name, bool isActive, int displayOrder)
  {
    var category = await _categoryRepository.GetByIdAsync(id);
   if(category is null) throw new KeyNotFoundException($"Category {id} was not found!");

  category.Name = name;
  category.IsActive = isActive;
  category.DisplayOrder = displayOrder;

  _categoryRepository.Update(category);
  await _categoryRepository.SaveChangesAsync();
  }

  public async Task DeleteAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
            throw new KeyNotFoundException($"Category {id} not found");

        _categoryRepository.Delete(category);
        await _categoryRepository.SaveChangesAsync();
    }
}