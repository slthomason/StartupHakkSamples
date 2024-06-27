using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleDataLayer.Entities
{
    public class BlogEntity
    {
        [Key] 
        public Int64 BlogId { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
