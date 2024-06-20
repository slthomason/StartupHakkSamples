public class JobEnqueuerService : IJobEnqueuerService
{
    private readonly IDatabase _redisDatabase;
    public JobEnqueuerService(IConnectionMultiplexer multiplexer)
    {
        _redisDatabase = multiplexer.GetDatabase();
    }
    // Add job at the back of the queue
public async Task EnqueueJobAsync(JobModel job)
{
    await _redisDatabase.ListLeftPushAsync("jobQueue", JsonSerializer.Serialize(job));
    await _redisDatabase.HashSetAsync(job.Id, "status", "queued");
}

// Fetch all jobs in the queue, along with their status
public async Task<List<JobModel>> GetJobsAsync()
{
    var jobs = await _redisDatabase.ListRangeAsync("jobQueue");
    var jobList = new List<JobModel>();
    foreach (var job in jobs)
    {
        var redisJob = JsonSerializer.Deserialize<JobModel>(job);
        redisJob.Status = _redisDatabase.HashGet(redisJob.Id, "status");
        jobList.Add(redisJob);
    }
    return jobList;
}
}