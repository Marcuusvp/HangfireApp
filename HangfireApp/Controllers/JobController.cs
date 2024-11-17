using Hangfire;
using Hangfire.Common;
using Microsoft.AspNetCore.Mvc;

namespace HangfireApp.Controllers;
[Route("api/[controller]")]
[ApiController]
public class JobController : ControllerBase
{
    [HttpPost]
    [Route("CriarRecurringJob")]
    public IActionResult CriarRecurringJob()
    {
        RecurringJob.AddOrUpdate("RecurringJob1", () => Console.WriteLine("Recurring Job"), Cron.Minutely);
        return Ok();
    }
    
    
    [HttpPost]
    [Route("CriarScheduledJob")]
    public IActionResult CriarScheduledJob()
    {
        var scheduledDateTime = DateTime.UtcNow.AddSeconds(5);
        var dateTimeOffSet = new DateTimeOffset(scheduledDateTime);
        
        var jobId = BackgroundJob.Schedule(() => Console.WriteLine("Job executed at " + dateTimeOffSet), dateTimeOffSet);
        
        var jobId2 = BackgroundJob.ContinueJobWith(jobId, () => Console.WriteLine("Segundo Job executed at " + dateTimeOffSet));
        
        var jobId3 = BackgroundJob.ContinueJobWith(jobId2, () => Console.WriteLine("Terceiro Job executed at " + dateTimeOffSet));
        
        var jobId4 = BackgroundJob.ContinueJobWith(jobId3, () => Console.WriteLine("Quarto Job executed at " + dateTimeOffSet));
        return Ok();
    }
    
    [HttpGet]
    public void ListaInteiros()
    {
        for (int i = 0; i < 100000; i++)
        {
            Console.WriteLine("Inteiro: " + i);
        }
    }
    
    [HttpPost]
    [Route("CriarBackgroundJob")]
    public IActionResult CriarBackgroundJob()
    {
        BackgroundJob.Enqueue(() => ListaInteiros());
        return Ok();
    }
}