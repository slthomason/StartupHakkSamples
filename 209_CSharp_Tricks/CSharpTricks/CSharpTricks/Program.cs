using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace CSharpTricks
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // 1. Using Records
            var recordProduct = new ProductRecord(1, "Example Product", 10, Status.Ordered, true);
            Console.WriteLine($"Record Product: {recordProduct.Title}");

            // 2. Tuple Pattern Matching
            var product = GetProductById(2);
            Console.WriteLine(GetShortDescription(product));

            // 3. Avoid Returning Null
            var availableProducts = GetAvailableProducts();
            Console.WriteLine($"Available Products Count: {availableProducts.Count()}");

            // 4. Unneeded Context
            var productsClass = new Products();
            productsClass.Add(new Product { Id = 3, Title = "Sugar", Status = Status.Delivered, Stock = 67, Available = true });
            productsClass.Delete(3);

            // 5. Parse JSON Stream
            var productsFromJson = await ParseJsonStream("myjson.json");
            Console.WriteLine($"Parsed Products Count: {productsFromJson.Count}");

            Console.ReadLine();
        }

        // 1. Record Example
        public record ProductRecord(int Id, string Title, int Stock, Status Status, bool Available);

        // 2. Tuple Pattern Matching Example
        public static string GetShortDescription(Product product) => (product.Available, product.Status) switch
        {
            (true, Status.Ordered) => $"{product.Title} has been ordered, but is still available.",
            (false, Status.Ordered) => $"{product.Title} is not in stock, but it has been ordered.",
            (false, Status.Delayed) => $"Order of {product.Title} has been delayed and there is no stock.",
            (true, Status.Delayed) => $"Order of {product.Title} has been delayed, but there is stock.",
            (false, Status.Delivered) => $"The {product.Title} has been delivered, meaning there is stock again.",
            (true, Status.Delivered) => $"The {product.Title} has been delivered, but there still is stock.",
            _ => "Unknown status"
        };

        // 3. Avoid Returning Null Example
        public static IEnumerable<Product> GetAvailableProducts()
        {
            List<Product> availableProducts = products.Where(x => x.Available).ToList();
            return availableProducts.Any() ? availableProducts : Enumerable.Empty<Product>();
        }

        // 4. Unneeded Context Example
        public class Products
        {
            private readonly List<Product> _products = new()
            {
                new Product { Id = 1, Title = "7Up", Status = Status.Ordered, Stock = 10, Available = true },
                new Product { Id = 2, Title = "Chips", Status = Status.Ordered, Stock = 0, Available = true }
            };

            public Product GetSingleProductById(int id)
            {
                return _products.SingleOrDefault(x => x.Id == id);
            }

            public void Add(Product product)
            {
                _products.Add(product);
            }

            public void Delete(int id)
            {
                var productToDelete = GetSingleProductById(id);
                if (productToDelete != null)
                {
                    _products.Remove(productToDelete);
                }
            }
        }

        // 5. Parse JSON Stream Example
        public static async Task<List<Product>> ParseJsonStream(string filePath)
        {
            using var stream = File.OpenRead(filePath);
            JsonNode node = JsonNode.Parse(stream);
            return node.Deserialize<List<Product>>();
        }

        public static Product GetProductById(int id)
        {
            return products.SingleOrDefault(p => p.Id == id);
        }

        public static List<Product> products = new()
        {
            new() { Id = 1, Title = "7Up", Status = Status.Ordered, Stock = 10, Available = true },
            new() { Id = 2, Title = "Chips", Status = Status.Ordered, Stock = 0, Available = true },
            new() { Id = 3, Title = "Sugar", Status = Status.Delivered, Stock = 67, Available = true },
            new() { Id = 4, Title = "Meatballs", Status = Status.Delivered, Stock = 5, Available = true },
            new() { Id = 5, Title = "Milk", Status = Status.Delivered, Stock = 7, Available = true },
            new() { Id = 6, Title = "Chewinggum", Status = Status.Delivered, Stock = 0, Available = false },
            new() { Id = 7, Title = "Toiletpaper", Status = Status.Delivered, Stock = 1, Available = true },
            new() { Id = 8, Title = "Tea", Status = Status.Delivered, Stock = 5, Available = true },
            new() { Id = 9, Title = "Coffee", Status = Status.Delivered, Stock = 85, Available = true },
            new() { Id = 10, Title = "Biscuits", Status = Status.Delivered, Stock = 12, Available = true },
            new() { Id = 11, Title = "Chocolate", Status = Status.Delivered, Stock = 89, Available = true },
            new() { Id = 12, Title = "Bread", Status = Status.Delivered, Stock = 167, Available = true },
        };
    }

    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Stock { get; set; }
        public Status Status { get; set; }
        public bool Available { get; set; }
    }

    public enum Status
    {
        Ordered,
        Delivered,
        Delayed,
        Unknown
    }
}
