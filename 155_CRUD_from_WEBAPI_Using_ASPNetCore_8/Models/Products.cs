namespace WebApplication24.Models
{
    public class Products
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool isActive { get; set; } = true;
    }
}
