namespace BackgroundService.Services;

public class ImplementIHostedService : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(async () =>
        {
            while(!cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine($"Response from IHostedService - {DateTime.Now}");
                await Task.Delay(1000);
            }
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Shutting down IHostedService");
        return Task.CompletedTask;
    }
}
