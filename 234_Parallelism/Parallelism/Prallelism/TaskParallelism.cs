using System;

namespace Prallelism;

using System;
using System.Threading.Tasks;

public static class TaskParallelism
{
    public static void Main()
    {
        Task task1 = Task.Run(() => PerformTask("Task 1", 2000));
        Task task2 = Task.Run(() => PerformTask("Task 2", 1000));
        Task task3 = Task.Run(() => PerformTask("Task 3", 1500));

        // Wait for all tasks to complete
        Task.WaitAll(task1, task2, task3);
    }

    static void PerformTask(string taskName, int delay)
    {
        Console.WriteLine($"{taskName} started.");
        Task.Delay(delay).Wait(); // Simulating work
        Console.WriteLine($"{taskName} completed.");
    }
}

