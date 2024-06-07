//Example Class
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

List<Product> products = new()
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

// Records

public class Product
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int Stock { get; set; }
    public Status Status { get; set; }
    public bool Available { get; set; }
}

//But let’s stick to the tip & trick. We can convert this class into a record, making the object way smaller:

public record Product(int Id, string Title, int Stock, Status Status, bool Available);


//Tuple Pattern Matching

Product product = products[2];
if (product.Available && product.Status == Status.Delivered)
{
    Console.WriteLine($"The {product.Title} has been delivered, but there still is stock.");
}
else if (!product.Available && product.Status == Status.Delivered)
{
    Console.WriteLine($"The {product.Title} has been delivered, meaning there is stock again.");
}
else if (product.Available && product.Status == Status.Delayed)
{
    Console.WriteLine($"Order of {product.Title} has been delayed, but there is stock.");
}
else if (!product.Available && product.Status == Status.Delayed)
{
    Console.WriteLine($"Order of {product.Title} has been delayed and there is no stock.");
}
else if (product.Available && product.Status == Status.Ordered)
{
    Console.WriteLine($"{product.Title} has been ordered, but is still available.");
}
else if (!product.Available && product.Status == Status.Ordered)
{
    Console.WriteLine($"{product.Title} is not in stock, but it has been ordered.");
}

//This is a bit much and we can make it shorter with Tuple pattern matching. This is an expression and switch glued together and it looks like this:

Console.WriteLine(GetShortDescription(products[2]));

string GetShortDescription(Product product) => (product.Available, product.Status) switch
{
    (true, Status.Ordered) => $"{product.Title} has been ordered, but is still available.",
    (false, Status.Ordered) => $"{product.Title} is not in stock, but it has been ordered.",
    (false, Status.Delayed) => $"Order of {product.Title} has been delayed and there is no stock.",
    (true, Status.Delayed) => $"Order of {product.Title} has been delayed, but there is stock.",
    (false, Status.Delivered) => $"The {product.Title} has been delivered, meaning there is stock again.",
    (true, Status.Delivered) => $"The {product.Title} has been delivered, but there still is stock.",
};

//No errors or warnings, right? Now do the same with the tuple-matching pattern:

Console.WriteLine(GetShortDescription(products[2]));

string GetShortDescription(Product product) => (product.Available, product.Status) switch
{
    (true, Status.Ordered) => $"{product.Title} has been ordered, but is still available.",
    (false, Status.Ordered) => $"{product.Title} is not in stock, but it has been ordered.",
    (false, Status.Delayed) => $"Order of {product.Title} has been delayed and there is no stock.",
    (true, Status.Delayed) => $"Order of {product.Title} has been delayed, but there is stock.",
    (false, Status.Delivered) => $"The {product.Title} has been delivered, meaning there is stock again.",
    (true, Status.Delivered) => $"The {product.Title} has been delivered, but there still is stock.",
    (true, Status.Delivered) => $"The {product.Title} has been delivered, but there still is stock.",
};


//Avoid Returning Null

public IEnumerable<Product> GetAvailableProducts()
{
    List<Product> availableProducts = products.Where(x => x.Available).ToList();

    if (!availableProducts.Any())
    {
        return Enumerable.Empty<Product>();
    }
    else
    {
        return availableProducts;
    }
}

//Or if you want to go nuts on writing less code:

public IEnumerable<Product> GetAvailableProducts()
{
    List<Product> availableProducts = products.Where(x => x.Available).ToList();

    return !availableProducts.Any() 
        ? Enumerable.Empty<Product>()
        : availableProducts;
}

//Unneeded Context (Version: since the dawn of OOP)

public class Products
{
    List<Product> products = new()
    {
        new() { Id = 1, Title = "7Up", Status = Status.Ordered, Stock = 10, Available = true },
        new() { Id = 2, Title = "Chips", Status = Status.Ordered, Stock = 0, Available = true },
    };

    public Product GetSingleProductById(int id)
    {
        Product productToReturn = products.SingleOrDefault(x => x.Id == id);
        return productToReturn;
    }

    public void DeleteAProduct(int id)
    {
        Product productToDelete = GetSingleProductById(id);
        products.Remove(productToDelete);
    }
}

//If I would want to delete a product I would use the following code:

new Products().DeleteAProduct(1);


//Better would be to rename the DeleteAProduct to Delete:

public Product GetSingleProductById(int id)
{
    Product productToReturn = products.SingleOrDefault(x => x.Id == id);
    return productToReturn;
}

new Products().Delete(1);

//Parse A JSON Stream

using var stream = File.OpenRead('myjson.json');
JsonNode node = await JsonNode.ParseAsync(stream);
List<Product> productsFromJson = node.GetValue<List<Product>>();
