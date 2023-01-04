using lc.df.api.Models;
using lc.df.api.Services;
using lc.df.api.Services.Models;

namespace lc.df.api.Jobs;

internal class SimpleDataJob : BackgroundService
{
    private readonly ILogger<SimpleDataJob> _logger;
    private readonly Subscription _subscription;

    public SimpleDataJob(IDataService service, ILogger<SimpleDataJob> logger)
    {
        _logger = logger;
        _subscription = service.Subscribe<SimpleModel>(ConsumeSimpleData);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Execute");
        return Task.CompletedTask;
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stops");
        _subscription.UnSubscribe();
        return base.StopAsync(cancellationToken);
    }

    private void ConsumeSimpleData(SimpleModel data)
    {
        _logger.LogInformation("Got {Id} with {Name}", data.Id, data.Name);
    }
}