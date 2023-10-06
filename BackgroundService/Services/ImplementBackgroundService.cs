using Hosting = Microsoft.Extensions.Hosting;

namespace BackgroundService.Services;

public class ImplementBackgroundService : Hosting.BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while(!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine($"Response from BackgroundService - {DateTime.Now}");
            await Task.Delay(1000);
        }
    }
}
