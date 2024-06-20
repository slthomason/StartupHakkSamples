//Use StringBuilder for String Manipulation
// Inefficient way
string result = "";
for (int i = 0; i < 1000000; i++)
{
    result += i.ToString();
}

// Better way
StringBuilder sb = new StringBuilder();
for (int i = 0; i < 1000000; i++)
{
    sb.Append(i.ToString());
}
string result = sb.ToString();




//Optimize LINQ Queries
// Inefficient LINQ query
var result = myList.Where(item => item.SomeProperty == someValue)
                   .OrderBy(item => item.AnotherProperty)
                   .ToList();

// Optimized query
var result = myList.Where(item => item.SomeProperty == someValue)
                   .ToList();




//Minimize Object Instantiation
// Inefficient object instantiation
for (int i = 0; i < 1000; i++)
{
    var obj = new MyObject();
    // Do something with obj
}

// Better approach - instantiate once
var obj = new MyObject();
for (int i = 0; i < 1000; i++)
{
    // Do something with obj
}




//Avoid Boxing and Unboxing
// Boxing example
object boxed = 10; // Boxing occurs here

// Unboxing example
int unboxed = (int)boxed; // Unboxing occurs here




//Implement Asynchronous Programming
// Synchronous method
public void DoSomething()
{
    // Perform synchronous operation
}

// Asynchronous method
public async Task DoSomethingAsync()
{
    // Perform asynchronous operation
}




//Memory Management
// Inefficient memory allocation
byte[] buffer = new byte[1000000]; // Allocates a large buffer unnecessarily

// Better approach - allocate memory only when needed
byte[] buffer = null;
if (condition)
{
    buffer = new byte[1000000];
}





//Use Value Types Where Appropriate
// Reference type example
MyClass referenceType = new MyClass();

// Value type example
int valueType = 10;




//Optimize Loops
// Inefficient loop
for (int i = 0; i < myList.Length; i++)
{
    // Do something with myList[i]
}

// Optimized loop
int length = myList.Length;
for (int i = 0; i < length; i++)
{
    // Do something with myList[i]
}


//Reduce Interop Calls
// Inefficient interop call
foreach (var item in collection)
{
    // Call unmanaged code for each item
    InteropMethod(item);
}

// Optimized interop call
var batchedItems = collection.ToArray();

// Batched call to unmanaged code
InteropMethodBatch(batchedItems);





//Optimize Database Access
// Using MemoryCache to cache frequently accessed data
MemoryCache cache = new MemoryCache(new MemoryCacheOptions());
string key = "CachedData";
if (!cache.TryGetValue(key, out string cachedData))
{
    // Data not in cache, retrieve it from source
    cachedData = GetDataFromSource();
    cache.Set(key, cachedData, TimeSpan.FromMinutes(10)); // Cache data for 10 minutes
}




//Use Structs for Small Data Structures
// Class example
class Point
{
    public int X { get; set; }
    public int Y { get; set; }
}

// Struct example
struct Point
{
    public int X { get; set; }
    public int Y { get; set; }
}




//Avoid Excessive Exception Handling
// Excessive exception handling
try
{
    // Code that may throw exceptions
}
catch (Exception ex)
{
    // Handle exception
}

// Minimal exception handling
try
{
    // Code that may throw exceptions
}
catch (SpecificException ex)
{
    // Handle specific exception
}





//Optimize Resource Usage
// Inefficient resource usage
var file = File.OpenRead("file.txt");
// Process file
file.Close(); // Resource not properly disposed

// Optimized resource usage
using (var file = File.OpenRead("file.txt"))
{
    // Process file
}