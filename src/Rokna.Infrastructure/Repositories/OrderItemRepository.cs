using Microsoft.EntityFrameworkCore;
using Rokna.Domain.Entities;
using Rokna.Domain.Interfaces;
using Rokna.Infrastructure.Data;

namespace Rokna.Infrastructure.Repositories;

public class OrderItemRepository : IOrderItemRepository
{
  private readonly RoknaDbContext _context;

  public OrderItemRepository(RoknaDbContext context)
  {
   _context = context;
  }

   public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
  {
        await _context.SaveChangesAsync(cancellationToken);
  }

    public async Task<IEnumerable<OrderItem>> GetByOrderIdAsync(int orderId, CancellationToken cancellationToken = default)
  {
   return await _context.OrderItems
                      .Where(oi => oi.OrderId == orderId)
                      .ToListAsync(cancellationToken);
  }

  public async Task AddAsync(OrderItem orderItem, CancellationToken cancellationToken = default)
  {
     await _context.OrderItems.AddAsync(orderItem, cancellationToken);
  }

}