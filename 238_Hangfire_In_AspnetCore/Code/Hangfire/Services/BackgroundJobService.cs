using Hangfire.Interface;
using Hangfire.Models;
using Microsoft.EntityFrameworkCore;

namespace Hangfire.Services;

public class BackgroundJobService : IBackgroundJobService
{
        private readonly IRecurringJobManager _recurringJobManager;
        private readonly DatabaseContext _context;


        public BackgroundJobService(IRecurringJobManager recurringJobManager,DatabaseContext _context)
        {
            _recurringJobManager = recurringJobManager;
            this._context = _context;
        }
        private string GenerateRandomCode()
        {
            Random random = new Random();
            return $"Code_{random.Next(1000, 9999)}";
        }
        public async Task AddThana()
        {
            Console.WriteLine(GenerateRandomCode());
            await _context.categories.ToListAsync();
        }

        public async Task ScheduleJobs()
        {
            // Schedule the recurring job here
            _recurringJobManager.AddOrUpdate<IBackgroundJobService>(
                "addThana", 
                service => service.AddThana(), 
                Cron.Minutely);
        }
}
