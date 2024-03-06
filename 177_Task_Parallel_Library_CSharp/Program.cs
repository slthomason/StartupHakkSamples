using System;
using System.Threading.Tasks;

//create a new task called task1
Task task1 = Task.Run(() => {
    return true;
});

//use value of task1
bool result = task1.Result;

// This blocks wait until task1 is completed
task1.Wait(); 

// This blocks wait until all tasks of the taskArray are completed
Task.WaitAll(taskArray);

// This blocks wait until any task of the taskArray is completed
Task.WaitAny(taskArray);

//create a task called task1 and return integer value 
Task<int> task1 = Task.Run(() => 42);

//create task2 with a continuation on task1
Task<string> task2 = task1.ContinueWith(previousTask => {
    //get the task1 returning data
    int result = previousTask.Result;
    return $"previous task return value is {result}";
});

Task.Factory.StartNew(() => DoComputation(1.0));

private static Double DoComputation(Double start)
{
      Double sum = 0;
      for (var value = start; value <= start + 10; value += .1)
         sum += value;

      return sum;
 }

var outer = Task.Factory.StartNew(() =>
{
    Console.WriteLine("Outer task beginning.");

    var child = Task.Factory.StartNew(() =>
    {
        Thread.SpinWait(5000000);
        Console.WriteLine("Detached task completed.");
    });
});

outer.Wait();
Console.WriteLine("Outer task completed.");

// The example displays the following output:
//    Outer task beginning.
//    Outer task completed.
//    Detached task completed.


public static void Main()
{
      var parent = Task.Factory.StartNew(() => {
                      Console.WriteLine("Parent task beginning.");
                      for (int ctr = 0; ctr < 10; ctr++) {
                         int taskNo = ctr;
                         Task.Factory.StartNew((x) => {
                                                  Thread.SpinWait(5000000);
                                                  Console.WriteLine("Attached child #{0} completed.",
                                                                    x);
                                               },
                                               taskNo, TaskCreationOptions.AttachedToParent);
                      }
                   });

      parent.Wait();
      Console.WriteLine("Parent task completed.");
}

// The example displays output like the following:
//       Parent task beginning.
//       Attached child #9 completed.
//       Attached child #0 completed.
//       Attached child #8 completed.
//       Attached child #1 completed.
//       Attached child #7 completed.
//       Attached child #2 completed.
//       Attached child #6 completed.
//       Attached child #3 completed.
//       Attached child #5 completed.
//       Attached child #4 completed.
//       Parent task completed.


public static partial class Program
{
    public static void HandleThree()
    {
        var task = Task.Run(
            () => throw new CustomException("This exception is expected!"));

        try
        {
            task.Wait();
        }
        catch (AggregateException ae)
        {
            foreach (var ex in ae.InnerExceptions)
            {
                // Handle the custom exception.
                if (ex is CustomException)
                {
                    Console.WriteLine(ex.Message);
                }
                // Rethrow any other exception.
                else
                {
                    throw ex;
                }
            }
        }
    }
}

// The example displays the following output:
//        This exception is expected!


public static partial class Program
{
    public static void FlattenTwo()
    {
        var task = Task.Factory.StartNew(() =>
        {
            var child = Task.Factory.StartNew(() =>
            {
                var grandChild = Task.Factory.StartNew(() =>
                {
                    // This exception is nested inside three AggregateExceptions.
                    throw new CustomException("Attached child2 faulted.");
                }, TaskCreationOptions.AttachedToParent);

                // This exception is nested inside two AggregateExceptions.
                throw new CustomException("Attached child1 faulted.");
            }, TaskCreationOptions.AttachedToParent);
        });

        try
        {
            task.Wait();
        }
        catch (AggregateException ae)
        {
            foreach (var ex in ae.Flatten().InnerExceptions)
            {
                if (ex is CustomException)
                {
                    Console.WriteLine(ex.Message);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
// The example displays the following output:
//    Attached child1 faulted.
//    Attached child2 faulted.


public static void Main()
{
        //Create array of tasks
        Task[] taskArray = new Task[10];
        
        //Assign, create and run tasks into task array
        for (int i = 0; i < taskArray.Length; i++)
        {
            taskArray[i] = Task.Factory.StartNew((Object obj) =>
            {
                CustomData data = obj as CustomData;
                if (data == null) return;

                data.ThreadNum = Thread.CurrentThread.ManagedThreadId;
            },
            new CustomData() { Name = i, CreationTime = DateTime.Now.Ticks });
        }

        //This waits until all tasks of the taskArray are completed
        Task.WaitAll(taskArray);

        
        foreach (var task in taskArray)
        {
            var data = task.AsyncState as CustomData;
            if (data != null)
                Console.WriteLine("Task #{0} created at {1}, ran on thread #{2}.",
                                  data.Name, data.CreationTime, data.ThreadNum);
        }
}

// The example displays output like the following:
//     Task #0 created at 635116412924597583, ran on thread #3.
//     Task #1 created at 635116412924607584, ran on thread #4.
//     Task #2 created at 635116412924607584, ran on thread #4.
//     Task #3 created at 635116412924607584, ran on thread #4.
//     Task #4 created at 635116412924607584, ran on thread #3.
//     Task #5 created at 635116412924607584, ran on thread #3.
//     Task #6 created at 635116412924607584, ran on thread #4.
//     Task #7 created at 635116412924607584, ran on thread #4.
//     Task #8 created at 635116412924607584, ran on thread #3.
//     Task #9 created at 635116412924607584, ran on thread #4.