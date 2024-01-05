//Custom Extension Methods

public static class StringExtensions
{
    public static IEnumerable<string> FilterByLength(this IEnumerable<string> source, int minLength)
    {
        return source.Where(s => s.Length >= minLength);
    }
}

//Aggregate Functions
//Count and Any

List<Product> products = GetProducts();

int highPriceCount = products.Count(p => p.Price > 100);
bool hasHighPrice = products.Any(p => p.Price > 100);

//Sum, Min, Max, and Average
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

int sum = numbers.Sum();
int min = numbers.Min();
int max = numbers.Max();
double average = numbers.Average();

List<Product> products = GetProducts();
decimal totalValue = products.Sum(p => p.Price);

//GroupBy Method

List<Product> products = GetProducts();

var groupedProducts = products.GroupBy(p => p.Category);

foreach (var group in groupedProducts)
{
    Console.WriteLine($"Category: {group.Key}");
    foreach (var product in group)
    {
        Console.WriteLine($"  Product: {product.Name}");
    }
}


//GroupJoin Method

List<Customer> customers = GetCustomers();
List<Order> orders = GetOrders();

var customerOrders = customers.GroupJoin(orders,
    customer => customer.Id,
    order => order.CustomerId,
    (customer, orderGroup) => new
    {
        Customer = customer,
        Orders = orderGroup
    });

foreach (var item in customerOrders)
{
    Console.WriteLine($"Customer: {item.Customer.Name}");
    foreach (var order in item.Orders)
    {
        Console.WriteLine($"  Order: {order.Id}");
    }
}

//Dynamic Sorting and Grouping

using System.Linq.Dynamic.Core;

List<Product> products = GetProducts();
string sortBy = "Price descending";

var sortedProducts = products.AsQueryable().OrderBy(sortBy);

//Using Dynamic LINQ Library
Install-Package System.Linq.Dynamic.Core

//Dynamic Expressions and Predicates
using System.Linq.Dynamic.Core;

List<Product> products = GetProducts();
string filter = "Price > 100";

var filteredProducts = products.AsQueryable().Where(filter);

//Deferred Execution Benefits
var productsQuery = products.Where(p => p.Price > 100);

// The query is not executed yet
Console.WriteLine("Query created.");

// The query is executed when the results are enumerated
foreach (var product in productsQuery)
{
    Console.WriteLine(product.Name);
}

//Immediate Execution Benefits

List<Product> expensiveProducts = products.Where(p => p.Price > 100).ToList();

// The query is executed immediately
Console.WriteLine("Expensive products retrieved.");


//Distinct and Reverse

List<int> numbers = new List<int> { 1, 2, 3, 3, 4, 5 };

var uniqueNumbers = numbers.Distinct(); // { 1, 2, 3, 4, 5 }
var reversedNumbers = numbers.Reverse(); // { 5, 4, 3, 3, 2, 1 }

//Skip and Take

int pageSize = 10;
int pageNumber = 2;

var pagedProducts = products.Skip((pageNumber - 1) * pageSize).Take(pageSize);

//Concat and Union

List<int> numbers1 = new List<int> { 1, 2, 3 };
List<int> numbers2 = new List<int> { 3,4, 5 };

var concatenated = numbers1.Concat(numbers2); // { 1, 2, 3, 3, 4, 5 }
var union = numbers1.Union(numbers2); // { 1, 2, 3, 4, 5 }

//Indexing and Partitioning

List<Product> products = GetProducts();

var indexedProducts = products
    .Select((product, index) => new { Product = product, Index = index })
    .Where(item => item.Index % 2 == 0)
    .Select(item => item.Product);

//Caching Results with AsEnumerable

var productsQuery = products.Where(p => p.Price > 100);

IEnumerable<Product> cachedResults = productsQuery.AsEnumerable();

// Using cachedResults will not re-execute the query

//Using Compiled Queries

using System.Data.Linq;
using System.Data.Linq.Mapping;

[Table(Name = "Products")]
public class Product
{
    // ...
}

public class MyDataContext : DataContext
{
    public Table<Product> Products;

    public MyDataContext(string connection) : base(connection) { }
}

static Func<MyDataContext, IQueryable<Product>> compiledQuery =
    CompiledQuery.Compile((MyDataContext context) =>
        from p in context.Products
        where p.Price > 100
        select p);

using (MyDataContext context = new MyDataContext(connectionString))
{
    var results = compiledQuery(context);
}

//Multi-Threading and Parallelism

using System.Linq.Parallel;

List<Product> products = GetProducts();

var expensiveProducts = products.AsParallel().Where(p => p.Price > 100);

//Querying Complex Data Types

XDocument xmlDoc = XDocument.Load("products.xml");

var products = xmlDoc.Descendants("Product")
    .Select(x => new
    {
        Name = x.Element("Name").Value,
        Price = decimal.Parse(x.Element("Price").Value)
    });

var expensiveProducts = products.Where(p => p.Price > 100);

//Stored Procedures and Functions


using System.Data.Linq;

public class MyDataContext : DataContext
{
    public Table<Product> Products;

    [Function(Name = "GetExpensiveProducts")]
    public ISingleResult<Product> GetExpensiveProducts([Parameter] decimal minPrice)
    {
        return ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), minPrice).ReturnValue as ISingleResult<Product>;
    }

    public MyDataContext(string connection) : base(connection) { }
}

using (MyDataContext context = new MyDataContext(connectionString))
{
    var expensiveProducts = context.GetExpensiveProducts(100);
}


//Transaction Management


using (MyDataContext context = new MyDataContext(connectionString))
{
    using (TransactionScope ts = new TransactionScope())
    {
        try
        {
            // Perform database operations

            context.SubmitChanges();
            ts.Complete();
        }
        catch (Exception ex)
        {
            // Handle exception
        }
    }
}