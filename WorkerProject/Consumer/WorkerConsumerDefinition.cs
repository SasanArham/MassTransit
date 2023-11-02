using MassTransit;
using System;
using System.Data.SqlTypes;

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
            // It means only exceptions of specified types are going to include in rety policy
            endpointConfigurator.UseMessageRetry(r => r.Interval(5, 1000).Handle<ArgumentException>());
            endpointConfigurator.UseMessageRetry(r => r.Interval(1, 1000).Handle<NullReferenceException>());


            // These are some other possible policies:
            //endpointConfigurator.UseMessageRetry(r => r.Immediate(5));
            //var initial = new TimeSpan(0, 1, 0);
            //var toAdd = new TimeSpan(0, 2, 0);
            //endpointConfigurator.UseMessageRetry(r => r.Incremental(5, initial, toAdd));
            //endpointConfigurator.UseMessageRetry(r => r.Interval(10, new TimeSpan(0, 10, 0)));
            //endpointConfigurator.UseMessageRetry(r => r.None());



            // It means if retry did not workd for ArgumentException , then try a second-level retry plicy in specified times
            endpointConfigurator.UseDelayedRedelivery(r =>
                r.Intervals(TimeSpan.FromHours(5), TimeSpan.FromHours(15), TimeSpan.FromHours(30))
                .Handle<ArgumentException>());

        }

    }
}
