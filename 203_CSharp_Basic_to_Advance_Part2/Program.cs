//9. Dictionaries
Dictionary<string, string> phonebook = new Dictionary<string, string>();
phonebook.Add("John Doe", "123-456-7890");
phonebook.Add("Jane Doe", "987-654-3210");

Console.WriteLine(phonebook["John Doe"]); // Output: 123-456-7890

//10. Methods
public class Rectangle
{
    public double Width { get; set; }
    public double Height { get; set; }

    public double GetArea()
    {
        return Width * Height;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var rect = new Rectangle();
        rect.Width = 5;
        rect.Height = 10;

        double area = rect.GetArea();
        Console.WriteLine($"Area of rectangle: {area}");
    }
}

//11. Classes & Objects
public class MyClass // Class definition
{
    public string PropertyName { get; set; } // Properties store data
    public void MethodName() { /*...*/ } // Methods define actions
}

MyClass obj = new MyClass(); // Object creation

//12. Exception Handling
public static int GetNumberInput()
{
  while (true)
  {
    try
    {
      Console.WriteLine("Enter a number: ");
      string input = Console.ReadLine();
      return int.Parse(input);
    }
    catch (FormatException)
    {
      Console.WriteLine("Invalid input. Please enter a number.");
    }
  }
}

public static void Main(string[] args)
{
  int number = GetNumberInput();
  Console.WriteLine($"You entered: {number}");
}

//13. Delegates, Events & Lambda

public delegate void MyDelegate(); // Delegate declaration

event MyDelegate MyEvent; // Event declaration

public class Person
{
  public string Name { get; set; }
  public int Age { get; set; }
}

public static void Main(string[] args)
{
  List<Person> people = new List<Person>()
  {
    new Person { Name = "Alice", Age = 30 },
    new Person { Name = "Bob", Age = 25 },
    new Person { Name = "Charlie", Age = 40 },
  };

  people.Sort((p1, p2) => p1.Name.CompareTo(p2.Name));

  foreach (var person in people)
  {
    Console.WriteLine(person.Name); // Output: Alice, Bob, Charlie (sorted by name)
  }
}

//14. LINQ (Language-Integrated Query)
using System.Linq;

public static void Main(string[] args)
{
  List<int> numbers = new List<int>() { 1, 2, 3, 4, 5, 6 };
  var evenNumbers = numbers.Where(x => x % 2 == 0);

  foreach (var number in evenNumbers)
  {
    Console.WriteLine(number); // Output: 2, 4, 6
  }
}

//15. Attributes
[Obsolete("Use the new DoSomethingV2 method instead.")]
public void DoSomething()
{
  // Implementation here
}

public void DoSomethingV2()
{
  // New and improved implementation
}

//16. Async/Await
using System.Threading.Tasks;

public static async Task DownloadFileAsync(string url, string filePath)
{
  // Simulate downloading data asynchronously
  await Task.Delay(2000); // Simulate a 2-second download

  // Write downloaded data to the file
  File.WriteAllText(filePath, "Downloaded content");
  Console.WriteLine($"File downloaded to: {filePath}");
}

public static void Main(string[] args)
{
  string url = "https://example.com/data.txt";
  string filePath = "downloaded_data.txt";

  DownloadFileAsync(url, filePath);

  // Continue program execution while download happens in the background
  Console.WriteLine("Downloading file...");
  Console.WriteLine("Meanwhile, you can do other things...");
}