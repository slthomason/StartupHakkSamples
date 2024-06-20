[ApiController]
[Route("[controller]")]
public class JobEnqueuerController : ControllerBase
{
    // Inject the service
    private readonly IJobEnqueuerService _jobEnqueuer;

    public JobEnqueuerController(IJobEnqueuerService jobEnqueuer)
    {
        _jobEnqueuer = jobEnqueuer;
    }

    [HttpPost]
    public async Task<IActionResult> EnqueueTask(JobModel job)
    {
        await _jobEnqueuer.EnqueueJobAsync(job);
        return Ok();
    }
    
    [HttpGet]
    public async Task<IActionResult> GetTasks()
    {
        var tasks = await _jobEnqueuer.GetJobsAsync();
        return Ok(tasks);
    }
}