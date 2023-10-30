using MassTransit;
using System;

namespace WorkerProject.Consumer
{
    public class WorkerConsumerDefinition : ConsumerDefinition<WorkerConsumer>
    {
        public WorkerConsumerDefinition()
        {
            // override the default endpoint name, for whatever reason
            this.EndpointName = "whatever-name-you-want";

            // limit the number of messages consumed concurrently
            // this applies to the consumer only, not the endpoint
            this.ConcurrentMessageLimit = 100;
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<WorkerConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Interval(5, 1000));
            // These are some other possible policies:

            //endpointConfigurator.UseMessageRetry(r => r.Immediate(5));

            //var initial = new TimeSpan(0, 1, 0);
            //var toAdd = new TimeSpan(0, 2, 0);
            //endpointConfigurator.UseMessageRetry(r => r.Incremental(5, initial, toAdd));

            //endpointConfigurator.UseMessageRetry(r => r.Interval(10, new TimeSpan(0, 10, 0)));

            //endpointConfigurator.UseMessageRetry(r => r.None());
        }

    }
}
