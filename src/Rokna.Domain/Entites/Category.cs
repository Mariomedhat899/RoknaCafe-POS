using System.ComponentModel.DataAnnotations;

namespace Rokna.Domain.Entities;

public class Category : BaseEntity
 {

  [MaxLength(50)]
  public string Name {get; set;} = string.Empty;

  public int DisplayOrder {get; set;}

  public bool IsActive {get; set;} = true;

  public ICollection<MenuItem> MenuItems {get; set;} = new List<MenuItem>();

 }