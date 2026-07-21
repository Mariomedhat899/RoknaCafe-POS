using Microsoft.EntityFrameworkCore;
using Rokna.Domain.Entities;
using Rokna.Domain.Interfaces;
using Rokna.Infrastructure.Data;

namespace Rokna.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{

  private readonly RoknaDbContext _context;

  public OrderRepository(RoknaDbContext context)
  {
   _context = context;
  }

  public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken = default)
  {
   return await _context.Orders
                       .Include(o => o.OrderItems)
                       .ThenInclude(oi => oi.MenuItem)
                       .ToListAsync(cancellationToken);
  }

  public async Task<IEnumerable<Order>> GetByDateRangeAsync(DateTime start, DateTime end, CancellationToken cancellationToken = default)
  {
   return await _context.Orders
                      .Where(o => o.DateTime >= start && o.DateTime <= end)
                      .Include(o => o.OrderItems)
                      .ThenInclude(oi => oi.MenuItem)
                      .ToListAsync(cancellationToken);
  }

  public async Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
  {
   return await _context.Orders
                      .Include(o => o.OrderItems)
                     .ThenInclude(oi => oi.MenuItem)
                     .FirstOrDefaultAsync(o => o.Id == id, cancellationToken); 
  }

  public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        await _context.Orders.AddAsync(order, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
      await _context.SaveChangesAsync(cancellationToken);
    }

    public void Update(Order order)
    {
        _context.Orders.Update(order);
    }

    public void Delete(Order order)
    {
        _context.Orders.Remove(order);
    }

}