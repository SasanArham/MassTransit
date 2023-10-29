using MassTransit;

namespace WorkerProject.Consumer
{
    public class WorkerConsumerDefinition  :ConsumerDefinition<WorkerConsumer>
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
        }

    }
}
