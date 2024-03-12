//Task.WaitAll
public static void WaitAll(Task[] tasks)
public static void WaitAll(params Task[] tasks)

var task1 = Task.Run(() => Console.WriteLine("Task 1"));
var task2 = Task.Run(() => Console.WriteLine("Task 2"));

Task.WaitAll(task1, task2);


//Task.WhenAll
public static Task WhenAll(Task[] tasks)
public static Task WhenAll(params Task[] tasks)

var task1 = Task.Run(() => Console.WriteLine("Task 1"));
var task2 = Task.Run(() => Console.WriteLine("Task 2"));

var whenAllTask = Task.WhenAll(task1, task2);
whenAllTask.ContinueWith((antecedent) =>
{
    Console.WriteLine("All tasks completed.");
});

//Task.WaitAll Real-time Use Case: Synchronous Data Aggregation 

public void FetchAllPlanetData()
{
    Task<string> marsDataTask = FetchPlanetDataAsync("Mars");
    Task<string> venusDataTask = FetchPlanetDataAsync("Venus");
    Task<string> jupiterDataTask = FetchPlanetDataAsync("Jupiter");

    Task.WaitAll(marsDataTask, venusDataTask, jupiterDataTask);

    // At this point, all tasks are complete. Time to compile data.
    var marsData = marsDataTask.Result;
    var venusData = venusDataTask.Result;
    var jupiterData = jupiterDataTask.Result;

    // Compile and process data...
}


// Task.WhenAll Real-time Use Case: Asynchronous Report Generation 

public async Task<string> CompileGalacticReportAsync()
{
    var marsDataTask = FetchPlanetDataAsync("Mars");
    var venusDataTask = FetchPlanetDataAsync("Venus");
    var jupiterDataTask = FetchPlanetDataAsync("Jupiter");

    await Task.WhenAll(marsDataTask, venusDataTask, jupiterDataTask);

    var report = $"Mars: {marsDataTask.Result}, Venus: {venusDataTask.Result}, Jupiter: {jupiterDataTask.Result}";

    return report;
}

//MultiTasking

Task[] tasks = new Task[]{
  DownloadDataAsync(), 
  ProcessFilesAsync(),
  SaveToDatabaseAsync()  
};

Task.WaitAll(tasks);

// Continue processing after all tasks finish

//Example: Downloading Files

async Task DownloadFilesAsync() {

  var downloadTasks = new List<Task>();
  
  // Start all downloads 
  foreach (var url in fileUrls) {
    var task = DownloadFileAsync(url);
    downloadTasks.Add(task);
  }

  // Wait for all downloads
  Task.WaitAll(downloadTasks.ToArray()); 
  
  // All files downloaded!
  AnalyzeFiles(); 
}

void AnalyzeFiles() {
  // Can safely read/process files here
}

// Task.WhenAll

var task1 = SomeLongTaskAsync();
var task2 = AnotherLongTaskAsync();

var all = Task.WhenAll(task1, task2);

await all; // No blocking!

// task1 & task2 completed


//Example: Downloading Files Async

async Task DownloadFilesAsync() {

  var downloadTasks = new List<Task>();
  
  // Start all downloads 
  foreach (var url in fileUrls) {
    var task = DownloadFileAsync(url); 
    downloadTasks.Add(task);
  }

  // Wait without blocking
  await Task.WhenAll(downloadTasks);
  
  // All files downloaded! 
  AnalyzeFiles();
} 

void AnalyzeFiles() {
  // Can read files safely
}