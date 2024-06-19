// Inject Redis Database
public class JobDequerService
{
private readonly IDatabase _redisDatabase;

public JobDequerService(IConnectionMultiplexer multiplexer)
{
    _redisDatabase = multiplexer.GetDatabase();
}

protected override async Task ExecuteAsync(CancellationToken stoppingToken)
{
    while (!stoppingToken.IsCancellationRequested)
    {
        var job = await DequeueJobAsync();
        if (job != null)
        {
            // Simulate time to complete job
            await Task.Delay(5000, stoppingToken);
            await CompleteJobAsync(job);
        }
    }
}
    public async Task<JobModel?> DequeueJobAsync()
{
    var job = await _redisDatabase.ListGetByIndexAsync("jobQueue", 0);
    if (!job.HasValue)
    {
        return null;
    }
    var redisJob = JsonSerializer.Deserialize<JobModel>(job);
     
    // change status from "queued" to "in progress"
    await _redisDatabase.HashSetAsync(redisJob.Id, "status", "in progress");
    return redisJob;
}

public async Task CompleteJobAsync(JobModel job)
{
    await _redisDatabase.ListRemoveAsync("jobQueue", JsonSerializer.Serialize(job));
    await _redisDatabase.KeyDeleteAsync(job.Id);
}
}
