using MassTransit;
using System.Threading;
using System.Threading.Tasks;

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
            
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<WorkerConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Interval(5, 1000));
            endpointConfigurator.PrefetchCount = 30;
            endpointConfigurator.ConcurrentMessageLimit = 5; // I dont know why it wont go over 10 even if I specify
        }

    }
}
