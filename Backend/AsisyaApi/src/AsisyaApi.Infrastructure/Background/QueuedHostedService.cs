using AsisyaApi.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AsisyaApi.Infrastructure.Background
{
    public class QueuedHostedService : BackgroundService
    {
        private readonly IBackgroundJobQueue _queue;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<QueuedHostedService> _logger;

        public QueuedHostedService(
            IBackgroundJobQueue queue,
            IServiceScopeFactory scopeFactory,
            ILogger<QueuedHostedService> logger)
        {
            _queue = queue;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var work = await _queue.DequeueAsync(stoppingToken);
                using var scope = _scopeFactory.CreateScope();

                try
                {
                    await work(scope.ServiceProvider, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Background job failed.");
                }
            }
        }
    }
}