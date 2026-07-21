using System.ComponentModel.DataAnnotations;

namespace Rokna.Domain.Entities;

public class MenuItem : BaseEntity
 {

[MaxLength(50)]
  public string Name {get; set;} = string.Empty;

  public decimal Price {get; set;}

  public int CategoryId {get; set;}

  public Category? Category {get; set;}

  public bool IsAvailable {get; set;} = true;

  [MaxLength(200)]
  public string? Notes {get; set;}

  public ICollection<OrderItem> OrderItems {get; set;} = new List<OrderItem>(); 
 }