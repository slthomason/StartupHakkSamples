using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultipleDatabases.Database.Entities;

public class Users
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int userId {get;set;}
    public string customerName {get;set;}
    public string phoneNumber {get;set;}
}
