using Microsoft.EntityFrameworkCore;
using Rokna.Domain.Entities;
using Rokna.Domain.Interfaces;
using Rokna.Infrastructure.Data;

namespace Rokna.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository 
{
  private readonly RoknaDbContext _context;

  public CategoryRepository(RoknaDbContext context)
  { 
   _context = context;
  }

  public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default)
  {
    return await _context.Categories.ToListAsync(cancellationToken);
  }

  public async Task<IEnumerable<Category>> GetActiveAsync(CancellationToken cancellationToken = default)
  {
    return await _context.Categories
                        .Where(c => c.IsActive)
                        .ToListAsync(cancellationToken);
  }

  public async Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
{
    return await _context.Categories.FindAsync(new object[] { id }, cancellationToken);
}

  public async Task AddAsync(Category category,CancellationToken cancellationToken = default)
  {
    await _context.Categories.AddAsync(category, cancellationToken);
  }

  public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
   {
    await _context.SaveChangesAsync(cancellationToken);
   }


    public void Update(Category category)
    {
        _context.Categories.Update(category);
    }

    public void Delete(Category category)
    {
        _context.Categories.Remove(category);
    }
}
