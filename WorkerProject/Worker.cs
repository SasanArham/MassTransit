namespace GettingStarted
{
    using System.Diagnostics.Contracts;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Contracts;
    using MassTransit;
    using Microsoft.Extensions.Hosting;
    using WorkerProject;
    using Microsoft.Extensions.DependencyInjection;
    using MassTransit.Transports;
    using MassTransit.Configuration;

    public class Worker : BackgroundService
    {
        //readonly IBus _bus;
        private  IPublishEndpoint _publishEndpoint;
        private IDatabaseContext _context;
        private readonly IServiceProvider _serviceProvider;

        public Worker(
             IServiceProvider serviceProvider)
        {
            //_bus = bus;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                using IServiceScope scope = _serviceProvider.CreateScope();
                _context = scope.ServiceProvider.GetRequiredService<IDatabaseContext>();
                _publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

                await _publishEndpoint.Publish(new GettingStarted { Value = $"The time is {DateTimeOffset.Now}" }, stoppingToken);
                await _context.SaveChangesAsync();

                await Task.Delay(5000, stoppingToken);
            }
        }
    }

}

