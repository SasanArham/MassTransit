using GettingStarted;
using GettingStarted.Contracts;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerProject.Consumer
{
    public class WorkerConsumer : IConsumer<GettingStarted.Contracts.GettingStarted>
    {

        public async Task Consume(ConsumeContext<GettingStarted.Contracts.GettingStarted> context)
        {
            Console.WriteLine(context.Message.Value);
        }
    }
}
