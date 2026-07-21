using Microsoft.EntityFrameworkCore;
using Rokna.Domain.Entities;
using Rokna.Domain.Interfaces;
using Rokna.Infrastructure.Data;

namespace Rokna.Infrastructure.Repositories;

public class MenuItemRepository : IMenuItemRepository
{
  private readonly RoknaDbContext _context;

  public MenuItemRepository(RoknaDbContext context) 
  {
   _context = context;
  }
  public async Task<IEnumerable<MenuItem>> GetByCategoryAsync(int categoryId, CancellationToken cancellationToken = default)
  {
    return await _context.MenuItems
                       .Where(m => m.CategoryId == categoryId)
                       .ToListAsync(cancellationToken);
  }

   public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    await _context.SaveChangesAsync(cancellationToken);
  }

    public async Task<IEnumerable<MenuItem>> GetAvailableAsync(CancellationToken cancellationToken = default)
  {
   return await _context.MenuItems
                       .Where(m => m.IsAvailable)
                       .ToListAsync(cancellationToken);
  }

  public async Task<MenuItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
  {
   return await _context.MenuItems.FindAsync(new object[] { id }, cancellationToken);
  }

  public async Task AddAsync(MenuItem menuItem, CancellationToken cancellationToken = default)
  {
   await _context.MenuItems.AddAsync(menuItem, cancellationToken);
  }

  public void Update(MenuItem menuItem)
  {
   _context.MenuItems.Update(menuItem);
  }

   public void Delete(MenuItem menuItem)
  {
   _context.MenuItems.Remove(menuItem);
  }

}