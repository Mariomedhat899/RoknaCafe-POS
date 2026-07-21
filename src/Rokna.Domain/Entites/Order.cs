using System.ComponentModel.DataAnnotations;

namespace Rokna.Domain.Entities;

public class Order : BaseEntity
 {
  [MaxLength(20)]
  public string OrderNumber {get; set;} = string.Empty;

  public DateTime DateTime {get; set;} = DateTime.Now;

  [MaxLength(100)]
  public string CafeName {get; set;} = "روكن هادي";

  public decimal TotalAmount {get;set;}

  public decimal? TaxAmount {get; set;}

  public decimal GrandTotal {get; set;}

  public OrderStatus Status {get; set;} = OrderStatus.Completed;

  [MaxLength(500)]
  public string? Notes {get; set;}

  public ICollection<OrderItem> OrderItems {get; set;} = new List<OrderItem>();
 }

 public enum OrderStatus
 {
         Active,
         Completed,
         Cancelled
 }