namespace WebApplication24.Models
{
    public class AddUpdateProducts
    {
        public required string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool isActive { get; set; } = true;
    }
}
