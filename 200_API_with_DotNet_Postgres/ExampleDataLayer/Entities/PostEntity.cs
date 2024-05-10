using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ExampleDataLayer.Entities
{
    public class PostEntity
    {
        [Key]
        public Int64 PostId { get; set; }
        public Int64 BlogId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string? Summary { get; set; }
        public DateOnly? PostedDate { get; set; }
    }
}
