using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EfCoreTips.Database.Entities;

public class Products
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public required int productId {get;set;}
    public required string productName {get;set;}
    
    [ForeignKey("catId")]
    public int catId {get;set;}
    public Categories? categories {get;set;}

    public bool isActive {get;set;}
}
