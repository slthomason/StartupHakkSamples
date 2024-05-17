//17. Miscellaneous
//ENUM
public enum DaysOfWeek
{
    Sunday,
    Monday,
    Tuesday,
    Wednesday,
    Thursday,
    Friday,
    Saturday
}

// Usage
DaysOfWeek today = DaysOfWeek.Wednesday;

//INTERFACE
public interface IVehicle
{
    void Start();
    void Stop();
}

// Implementing the interface in a class
public class Car : IVehicle
{
    public void Start()
    {
        Console.WriteLine("Car started.");
    }

    public void Stop()
    {
        Console.WriteLine("Car stopped.");
    }
}

//CLASS
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }

    public void Introduce()
    {
        Console.WriteLine($"Hello, my name is {Name} and I am {Age} years old.");
    }
}

// Usage
Person person = new Person { Name = "Alice", Age = 30 };
person.Introduce();

//RECORD
public record PersonRecord(string Name, int Age);

// Usage
PersonRecord personRecord = new PersonRecord("Alice", 30);
Console.WriteLine(personRecord);

//STRUCT
public struct Point
{
    public int X { get; set; }
    public int Y { get; set; }
}

// Usage
Point point = new Point { X = 10, Y = 20 };
Console.WriteLine($"Point coordinates: X = {point.X}, Y = {point.Y}");

//DYNAMIC
dynamic value = "Hello, World!";
Console.WriteLine(value);  // No compile-time type checking

value = 42;
Console.WriteLine(value);  // Type can change at runtime

//IS
object obj = "This is a string";

if (obj is string str)
{
    Console.WriteLine($"The object is a string: {str}");
}

//AS
object obj = "This is a string";
string str = obj as string;

if (str != null)
{
    Console.WriteLine($"Successfully cast to string: {str}");
}

//VAR
var number = 42;  // inferred as int
var message = "Hello, World!";  // inferred as string

Console.WriteLine($"Number: {number}, Message: {message}");

//NAMEOF
public class Person
{
    public string Name { get; set; }
}

// Usage
Person person = new Person { Name = "Alice" };
Console.WriteLine(nameof(person.Name));  // Output: "Name"


//18. String Manipulation
string.Concat(); // Combine strings
string.Join(); // Join elements
str.Split(); // Split string
str.ToUpper(); // Convert to uppercase
str.ToLower(); // Convert to lowercase

//19. File I/O

using System.IO; // Required for File I/O

File.ReadAllText(path); // Read file content
File.WriteAllText(path, content); // Write to file
File.Exists(path); // Check file existence

//20. Date & Time
using System;

public static void Main(string[] args)
{
  DateTime startDate = DateTime.Parse("2024-03-10");
  DateTime endDate = DateTime.Now;

  TimeSpan difference = endDate - startDate;
  Console.WriteLine($"Time difference: {difference.Days} days, {difference.Hours} hours");
}

//21. Generics
public class Stack<T>
{
  private List<T> items = new List<T>();

  public void Push(T item)
  {
    items.Add(item);
  }

  public T Pop()
  {
    T item = items[items.Count - 1];
    items.RemoveAt(items.Count - 1);
    return item;
  }
}

public static void Main(string[] args)
{
  Stack<string> messages = new Stack<string>();
  messages.Push("Hello");
  messages.Push("World");

  string message = messages.Pop();
  Console.WriteLine(message); // Output: World
}

//22. Nullables
int? nullableInt = null; // Nullable integer


//23. Attributes & Reflection

public class Person
{
  public string Name { get; set; }
  public int Age { get; set; }
}

public static void Main(string[] args)
{
  Type personType = typeof(Person);
  PropertyInfo[] properties = personType.GetProperties();

  foreach (PropertyInfo property in properties)
  {
    Console.WriteLine(property.Name); // Output: Name, Age
  }
}

//24. Extension Methods
public static class StringExtensions
{
  public static string ToUppercase(this string str)
  {
    return str.ToUpper();
  }
}

public static void Main(string[] args)
{
  string message = "Hello, world!";
  string uppercased = message.ToUppercase(); // Using the extension method
  Console.WriteLine(uppercased); // Output: HELLO, WORLD!
}

//25. Dependency Injection
public interface ILogger
{
  void LogMessage(string message);
}

public class MyService
{
  private readonly ILogger _logger;

  public MyService(ILogger logger)
  {
    _logger = logger;
  }

  public void DoSomething()
  {
    _logger.LogMessage("Doing something...");
  }
}

// Implementing the ILogger interface (example)
public class ConsoleLogger : ILogger
{
  public void LogMessage(string message)
  {
    Console.WriteLine(message);
  }
}

public static void Main(string[] args)
{
  ILogger logger = new ConsoleLogger();
  MyService service = new MyService(logger);
  service.DoSomething();
}

//26. Partial Classes
public partial class MyClass { /*...*/ } // Partial class definition


//27. Interoperability
using System;
using System.Runtime.InteropServices;

[DllImport("user32.dll")]
public static extern int MessageBox(IntPtr hWnd, string lpText, string lpCaption, uint uType);

public static void Main(string[] args)
{
  MessageBox(IntPtr.Zero, "Hello from C#!", "Interop Example", 0);
}

//28. Anonymous Types

var person = new { Name = "John", Age = 30 };
Console.WriteLine($"Name: {person.Name}, Age: {person.Age}");

//29. Tuple
(string Name, int Age) person = ("Alice", 30);
Console.WriteLine($"Name: {person.Name}, Age: {person.Age}"); // Accessing elements using Item1 and Item2

//30. Pattern Matching
object obj = new Person { Name = "Bob", Age = 25 };

if (obj is Person { Name: "Bob", Age >= 18 })
{
  Console.WriteLine("Bob is an adult.");
}

//31. Local Functions

public static int Calculate(int number)
{
  int Factorial(int n)
  {
    if (n == 0) return 1;
    return n * Factorial(n - 1);
  }

  return Factorial(number);
}

public static void Main(string[] args)
{
  int result = Calculate(5);
  Console.WriteLine($"5! = {result}");
}

//32. Records
public record Person(string Name, int Age);

public static void Main(string[] args)
{
  Person person1 = new Person("Alice", 30);
  Person person2 = new Person("Alice", 30);

  // Records provide default equality comparison
  if (person1 == person2)
  {
    Console.WriteLine("People are equal");
  }
}

//33. with Expressions
var john = new Person("John", 30);
var jane = john with { Name = "Jane" }; // Non-destructive mutation

//34. Indexers and Ranges
int[] arr = {0, 1, 2, 3, 4, 5};
var subset = arr[1..^1]; // Indexer and range usage

//35. using Declaration
using var reader = new StreamReader("file.txt"); // using declaration

//36. Nullable Reference Types (NRTs)
public class Person
{
  public string Name { get; set; }
  public int Age { get; set; }
}

public static void Main(string[] args)
{
  Person person = new Person() { Age = 30 };

  // NRTs require null checks before accessing properties
  if (person?.Name != null)
  {
    Console.WriteLine(person.Name);
  }
  else
  {
    Console.WriteLine("Name is null");
  }
}

//37. Pattern-Based Using
public ref struct ResourceWrapper { /*...*/ } // Resource wrapper

using var resource = new ResourceWrapper(); // Pattern-based using

//38. Property Patterns
if (obj is Person { Name: "John", Age: var age }) { /*...*/ } // Property pattern matching


//39. Default Interface Implementations
public interface IPerson { /*...*/ } // Interface with default method
public class MyClass : IPerson { /*...*/ } // Class implementing interface

//40. Dynamic Binding
dynamic d = 5; // Dynamic binding
d = "Hello"; // No compile-time type checking