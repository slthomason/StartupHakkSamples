public void Enumerate(IEnumerable<int> enumerable) 
{
    foreach (var item in enumerable)
    {
        Console.WriteLine(item);
    }
}


public void Enumerate(IEnumerable<int> enumerable) 
{
    using(var enumerator = enumerable.GetEnumerator())
    {
        while(enumerator.MoveNext())
        {
            var item = enumerator.Current;
            Console.WriteLine(item);
        }
    }
}

//Count() vs. Any()

public static double? Average(this IEnumerable<int> enumerable) 
{
    if(enumerable.Count() == 0)
        return null;

    var sum = 0;
    foreach (var value in enumerable)
        sum += value;

    return sum / (double)enumerable.Count();
}

public static int MyCount<T>(this IEnumerable<T> enumerable) 
{
    var count = 0;
    using(var enumerator = enumerable.GetEnumerator())
    {
        while (enumerator.MoveNext())
            count++;
    }
    return count;
}

public static bool MyAny<T>(this IEnumerable<T> enumerable) 
{
  using (var enumerator = enumerable.GetEnumerator())
  {
      return enumerator.MoveNext();
  }
}

public void DoSomething<T>(IEnumerable<T> enumerable) 
{
    if (!enumerable.Any())
        return;

    // do something here
}

public static double? Average(this IEnumerable<int> enumerable) 
{
    var sum = 0;
    var count = 0;
    foreach (var value in enumerable)
    {
        sum += value;
        count++;
    }
    
    if(count == 0)
        return null;

    return sum / (double)count;
}

//Count() vs. Count

public static IReadOnlyList<int> MyRange(int start, int count)
{
    if (count < 0)
        throw new ArgumentOutOfRangeException(nameof(count));

    var range = new List<int>(); // List<int> implements IReadOnlyList<int>
    var end = start + count;
    for (int value = start; value < end; value++)
    {
        range.Add(value);
    }
    return range;
}

public static double? Average(this IReadOnlyCollection<int> enumerable) 
{
    if (enumerable.Count == 0)
        return null;
    
    var sum = 0;
    foreach (var value in enumerable)
        sum += value;
    
    return sum / (double)enumerable.Count;
}

//Yield
public static IEnumerable<int> MyRange(int start, int count)
{
    if (count < 0)
        throw new ArgumentOutOfRangeException(nameof(count));

    var end = start + count;
    for (int value = start; value < end; value++)
    {
        yield return value;
    }
}



public static IEnumerable<int> MyRange(int start, int count)
{
    if (count < 0)
        throw new ArgumentOutOfRangeException(nameof(count));

    return RangeEnumeration(start, start + count);

    // static local function to avoid closures
    static IEnumerable<int> RangeEnumeration(int start, int end)
    {
        for (var value = start; value < end; value++)
        {
            yield return value;
        }
    }
}

//Composition

var numbers = Enumerable.Range(start, count)
    .ToList();

var evenNumbers = numbers
    .Where(number => (number & 0x01) == 0)
    .ToList();

var oddNumbers = numbers
    .Where(number => (number & 0x01) != 0)
    .ToList();

Console.WriteLine("Even Numbers:");
foreach(var number in evenNumbers)
    Console.WriteLine(number);

Console.WriteLine("Odd Numbers:");
foreach(var number in oddNumbers)
    Console.WriteLine(number);


var numbers = Enumerable.Range(start, count);
        
var evenNumbers = numbers
    .Where(number => (number & 0x01) == 0);

var oddNumbers = numbers
    .Where(number => (number & 0x01) != 0);

Console.WriteLine("Even Numbers:");
foreach(var number in evenNumbers)
    Console.WriteLine(number);

Console.WriteLine("Odd Numbers:");
foreach(var number in oddNumbers)
    Console.WriteLine(number);


//null vs. Enumerable.Empty<T>()

public static IEnumerable<int> MyEnumerable(int count)
{
    if(count <= 0)
        return Enumerable.Empty<int>();

    return Enumerable.Range(0, count);
}

public static IEnumerable<int> MyEnumerable(int count)
{
    if (count <= 0)
        yield break;

    for (var value = 0; value < count; value++)
        yield return value;
}

//IQueryable

using (var db = new BloggingContext())
{
    var blogs = db.Blogs
        .Where(blog => blog.Rating > 3)
        .OrderByDescending(blog => blog.Rating)
        .Take(5)
        .Select(blog => blog.Url);

    Console.WriteLine(blogs.ToSql());
    Console.WriteLine();

    foreach (var blog in blogs)
    {
        Console.WriteLine(blog);
    }
}

/*
SELECT TOP(5) [blog].[Url]
FROM [Blogs] AS [blog]
WHERE [blog].[Rating] > 3
ORDER BY [blog].[Rating] DESC

*/

using (var db = new BloggingContext())
{
    var blogs = db.Blogs
        .Where(blog => blog.Rating > 3)
        .OrderByDescending(blog => blog.Rating)
        .Take(5)
        .Select(blog => blog.Url);

    Console.WriteLine(blogs.ToSql());
    Console.WriteLine();

    var urls = blogs
        .AsEnumerable()
        .Select(url => $"URL: {url}");

    foreach (var url in urls)
    {
        Console.WriteLine(url);
    }
}