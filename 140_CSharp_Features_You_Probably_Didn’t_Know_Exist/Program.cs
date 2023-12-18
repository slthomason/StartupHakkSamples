//Tweaking File Paths   
public class FilePath
{
    private string path;

    public FilePath(string initialPath)
    {
        path = initialPath;
    }

    public static FilePath operator /(FilePath filePath, string addition)
    {
        return new FilePath(filePath.path + "/" + addition);
    }

    public override string ToString()
    {
        return path;
    }
}

// Usage
var basePath = new FilePath("C:/Users/YourUser");
var fullPath = basePath / "Documents" / "test.txt";
Console.WriteLine(fullPath);




// The ‘Await Anything’ Trick
public static class IntExtensions
{
    public static TaskAwaiter GetAwaiter(this int number)
    {
        return Task.Delay(number * 1000).GetAwaiter();
    }
}

// Usage
await 5; // Waits for 5 seconds

// For-Each Loops in New Ways

public static class IntRangeExtensions {
    public static IEnumerable<int> To(this int start, int end) {
        for (int i = start; i <= end; i++)
            yield return i;
    }
}

// Usage
foreach (var i in 1.To(5)) {
    Console.WriteLine(i); // Prints numbers 1 to 5
}


//Extensions on Basic Types

public static class IntExtensions {
    public static DateTime AsDate(this int day, int month, int year) => new DateTime(year, month, day);
}

// Usage
var date = 30.AsDate(11, 2023); // November 30, 2023
Console.WriteLine(date);

//Custom Lock Behavior

namespace System.Threading {
    public static class Monitor {
        public static void Enter(object obj, ref bool lockTaken) {
            Console.WriteLine("Entering lock");
            System.Threading.Monitor.Enter(obj, ref lockTaken);
        }
        public static void Exit(object obj) {
            Console.WriteLine("Exiting lock");
            System.Threading.Monitor.Exit(obj);
        }
    }
}

// Usage
object lockObj = new object();
bool lockTaken = false;
Monitor.Enter(lockObj, ref lockTaken);
// Critical section
Monitor.Exit(lockObj);

//The Crazy Async Chain

using System.Runtime.CompilerServices;

public class Async {
    public static AsyncMethodBuilder<Async> Create() => new AsyncMethodBuilder<Async>();
    public void AwaitOnCompleted() { /* custom logic */ }
}

// Usage
async async async async() => await async;