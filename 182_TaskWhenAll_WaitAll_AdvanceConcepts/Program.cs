//Exception Handling

try
{
    await Task.WhenAll(tasks);
}
catch (Exception ex)
{
    // Handle exception
}

//Cancellation

CancellationTokenSource cts = new CancellationTokenSource();

Task.WaitAll(tasks, cts.Token);

//or

Task.WhenAll(tasks, cts.Token);


//Performance Considerations
int[] numbers = Enumerable.Range(1, 1000000);

Parallel.ForEach(numbers, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, number =>
{
    Console.WriteLine(number);
});

async Task ProcessDataAsync()
{
    var tasks = data.Select(async item =>
    {
        await ProcessItemAsync(item);
    });

    await Task.WhenAll(tasks);
}


var taskGroup = new TaskGroup(tasks);

try
{
    await taskGroup.WhenAll();
}
catch (OperationCanceledException)
{
    // Handle cancellation
}

//Error Handling in Task Groups

var taskGroup = new TaskGroup(tasks);

taskGroup.HandleExceptions().Subscribe(aggregateException =>
{
    // Handle aggregate exception
});

//Performance Best Practices

// DON'T do this!
await Task.WhenAll(tasks);

// Blocking call ❌
ProcessFiles();

//Solution

await Task.WhenAll(tasks);

// Async all the way down ✅ 
await ProcessFilesAsync();


//Handle Exceptions

var task1 = ProcessDataAsync(); // throws Exception 💥
var task2 = SaveDataAsync();

try {
  await Task.WhenAll(task1, task2);

} catch (Exception ex) {
   // Handle aggregated exception
}

//Use Continuations

var task1 = DownloadFileAsync();

task1.ContinueWith(t => {
  // Runs after task1 
  UnzipFile(t.Result); 
});

// Extra logic without blocking


//Cancelling Task.WaitAll

var cts = new CancellationTokenSource();

var task1 = LongTaskAsync(cts.Token);
var task2 = AnotherLongTaskAsync(cts.Token);  

var tasks = new [] { task1, task2 };

try {
  Task.WaitAll(tasks); 

} catch (Exception ex) {
  // Aggregated exception
  if (ex is OperationCanceledException) {
    // Cancelled
  }
} 

// Cancel tasks 
cts.Cancel();

//Cancelling Task.WhenAll

var cts = new CancellationTokenSource();  

var task1 = LongTaskAsync(cts.Token);
var task2 = AnotherLongTaskAsync(cts.Token);

try {

  await Task.WhenAll(task1, task2);

} catch (Exception ex) {
  if (ex is OperationCanceledException) {   
    // Cancelled
  }  
}

// Cancel tasks
cts.Cancel();