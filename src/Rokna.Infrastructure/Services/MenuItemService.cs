using Rokna.Domain.Entities;
using Rokna.Domain.Interfaces;
using Rokna.Infrastructure.Repositories;

namespace Rokna.Infrastructure.Services;

public class MenuItemService : IMenuItemService
{
  private readonly IMenuItemRepository _menuItemRepository;

  public MenuItemService(IMenuItemRepository menuItemRepository)
  {
    _menuItemRepository = menuItemRepository;
  }

  public async Task<IEnumerable<MenuItem>> GetByCategoryAsync(int categoryId)
  {
    return await _menuItemRepository.GetByCategoryAsync(categoryId);
  }

  public async Task<IEnumerable<MenuItem>> GetAvailableAsync()
  {
    return await _menuItemRepository.GetAvailableAsync();
  }

  public async Task<MenuItem?> GetByIdAsync(int id)
  {
    return await _menuItemRepository.GetByIdAsync(id);
  }

  public async Task<IEnumerable<MenuItem>> GetAllAsync()
  {
    return await _menuItemRepository.GetAvailableAsync();
  }

  public async Task<MenuItem> CreateAsync(string name, decimal price, int categoryId)
  {
    var menuItem = new MenuItem
    {
     Name = name,
    Price = price,
    CategoryId = categoryId,
    IsAvailable  = true
    };

  await _menuItemRepository.AddAsync(menuItem);
  await _menuItemRepository.SaveChangesAsync();

  return menuItem;
  }

  public async Task UpdateAsync(int id, string name, decimal price, bool isAvailable, int categoryId)
  {
    var menuItem = await _menuItemRepository.GetByIdAsync(id);
    if (menuItem is null) throw new KeyNotFoundException($"Menu item with id:{id} was not found!");

    menuItem.Name = name;
    menuItem.Price = price;
    menuItem.IsAvailable = isAvailable;
    menuItem.CategoryId = categoryId;

    _menuItemRepository.Update(menuItem);
    await _menuItemRepository.SaveChangesAsync();
  }
  public async Task DeleteAsync(int id)
  {
    var menuItem = await _menuItemRepository.GetByIdAsync(id);
    if (menuItem == null)
        throw new KeyNotFoundException($"Menu item {id} not found");

    _menuItemRepository.Delete(menuItem);
    await _menuItemRepository.SaveChangesAsync();
  }
}