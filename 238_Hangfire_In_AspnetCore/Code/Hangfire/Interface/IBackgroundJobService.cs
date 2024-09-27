using System;

namespace Hangfire.Interface;

public interface IBackgroundJobService
{
   Task AddThana();
   Task ScheduleJobs();
}
