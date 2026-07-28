using Rokna.Domain.Entities;
using Rokna.Domain.Interfaces;
using Rokna.Infrastructure.Repositories;

namespace Rokna.Infrastructure.Services;

public class OrderService : IOrderService 
{
  private readonly IOrderRepository  _orderRepository;
  private readonly IOrderItemRepository _orderItemRepository;
  private  readonly IMenuItemRepository  _menuItemRepository;

  public OrderService(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, IMenuItemRepository menuItemRepository)
  {
   _orderRepository = orderRepository;
   _orderItemRepository = orderItemRepository;
  _menuItemRepository = menuItemRepository;
  }

  public async Task<IEnumerable<Order>> GetAllAsync()
  {
      return await _orderRepository.GetAllAsync();
  }

  public async Task<IEnumerable<Order>> GetByDateRangeAsync(DateTime start, DateTime end)
  {
      return await _orderRepository.GetByDateRangeAsync(start, end);
  }

  public async Task<Order?> GetByIdAsync(int id)
  {
      return await _orderRepository.GetByIdAsync(id);
  }

  public async Task<Order> CreateOrderAsync(string? orderNumber, string? notes, List<OrderItemRequest> items)
  {
      var order = new Order
      {
          OrderNumber = orderNumber ?? GenerateOrderNumber(),
          CafeName = "روكن هادي",
          DateTime = DateTime.Now,
          Status = OrderStatus.Active,
          Notes = notes ?? string.Empty
      };

      await _orderRepository.AddAsync(order);
      await _orderRepository.SaveChangesAsync();

      decimal total = 0;

      foreach (var item in items)
      {
          var menuItem = await _menuItemRepository.GetByIdAsync(item.MenuItemId);
          if (menuItem == null)
              throw new KeyNotFoundException($"Menu item {item.MenuItemId} not found");

          if (!menuItem.IsAvailable)
              throw new InvalidOperationException($"Menu item '{menuItem.Name}' is not available");

          if (item.UnitPrice != menuItem.Price)
              throw new InvalidOperationException($"Price mismatch for '{menuItem.Name}'. Expected {menuItem.Price}, got {item.UnitPrice}");

          var orderItem = new OrderItem
          {
              OrderId = order.Id,
              MenuItemId = menuItem.Id,
              Quantity = item.Quantity,
              UnitPrice = menuItem.Price,
              SubTotal = menuItem.Price * item.Quantity
          };

          await _orderItemRepository.AddAsync(orderItem);
          total += orderItem.SubTotal;
      }

      await _orderItemRepository.SaveChangesAsync();

      order.TotalAmount = total;
      _orderRepository.Update(order);
      await _orderRepository.SaveChangesAsync();

      return order;
  }

  public async Task<Order> CloseOrderAsync(int orderId, bool isPaid = true)
  {
      var order = await _orderRepository.GetByIdAsync(orderId);
      if (order == null)
          throw new KeyNotFoundException($"Order with id:{orderId} was not found!");

      order.Status = isPaid ? OrderStatus.Completed : OrderStatus.Cancelled;
      _orderRepository.Update(order);
      await _orderRepository.SaveChangesAsync();

      return order;
  }

  private string GenerateOrderNumber()
  {
      return $"RN-{DateTime.Now:yyyyMMddHHmmss}";
  }
  }