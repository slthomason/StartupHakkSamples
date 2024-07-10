using System;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Autofac;
using AutoMapper;
using Serilog;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;

namespace NuGetDemo
{
    // Product class for JSON serialization/deserialization
    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    // Blog and BloggingContext for EntityFramework
    public class Blog
    {
        public int BlogId { get; set; }
        public string Name { get; set; }
    }

    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("BloggingDb");
        }
    }

    // Math class for xUnit testing
    public class Math
    {
        public int Add(int x, int y) => x + y;
    }

    // Customer class and validator for FluentValidation
    public class Customer
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(customer => customer.Name).NotEmpty();
            RuleFor(customer => customer.Age).GreaterThan(18);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // 1. Newtonsoft.Json
            Console.WriteLine("Newtonsoft.Json Example:");
            var product = new Product { Name = "Apple", Price = 1.99M };
            string json = JsonConvert.SerializeObject(product);
            Console.WriteLine($"Serialized: {json}");
            var deserializedProduct = JsonConvert.DeserializeObject<Product>(json);
            Console.WriteLine($"Deserialized: {deserializedProduct.Name}, {deserializedProduct.Price}");
            Console.WriteLine();

            // 2. EntityFramework
            Console.WriteLine("EntityFramework Example:");
            using (var context = new BloggingContext())
            {
                var blog = new Blog { Name = "My Blog" };
                context.Blogs.Add(blog);
                context.SaveChanges();
                var savedBlog = context.Blogs.First();
                Console.WriteLine($"Blog Saved: {savedBlog.Name}");
            }
            Console.WriteLine();

            // 3. xUnit
            Console.WriteLine("xUnit Example:");
            var math = new Math();
            Console.WriteLine($"2 + 2 = {math.Add(2, 2)}");
            Console.WriteLine();

            // 4. Autofac
            Console.WriteLine("Autofac Example:");
            var builder = new ContainerBuilder();
            builder.RegisterType<MyComponent>().As<IMyComponent>();
            var container = builder.Build();
            var myComponent = container.Resolve<IMyComponent>();
            myComponent.DoSomething();
            Console.WriteLine();

            // 5. AutoMapper
            Console.WriteLine("AutoMapper Example:");
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Source, Destination>());
            IMapper mapper = config.CreateMapper();
            var source = new Source { Value = "Hello" };
            var destination = mapper.Map<Source, Destination>(source);
            Console.WriteLine($"Mapped Value: {destination.Value}");
            Console.WriteLine();

            // 6. Serilog
            Console.WriteLine("Serilog Example:");
            var log = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            log.Information("Hello, Serilog!");
            Console.WriteLine();

            // 7. FluentValidation
            Console.WriteLine("FluentValidation Example:");
            var customer = new Customer { Name = "", Age = 17 };
            var validator = new CustomerValidator();
            var result = validator.Validate(customer);
            Console.WriteLine(result.IsValid ? "Customer is valid" : "Customer is not valid");
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"Validation Error: {error.ErrorMessage}");
            }
        }
    }

    // Autofac Example Classes
    public interface IMyComponent
    {
        void DoSomething();
    }

    public class MyComponent : IMyComponent
    {
        public void DoSomething()
        {
            Console.WriteLine("MyComponent is doing something!");
        }
    }

    // AutoMapper Example Classes
    public class Source
    {
        public string Value { get; set; }
    }

    public class Destination
    {
        public string Value { get; set; }
    }
}
