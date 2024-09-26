using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultipleDatabases.Database.Entities;

public class Categories
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int catId {get;set;}
    public string catName {get;set;}
}
