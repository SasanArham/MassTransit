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
    public class WorkerConsumer : IConsumer<Batch<GettingStarted.Contracts.GettingStarted> >
    {

        public async Task Consume(ConsumeContext<Batch<GettingStarted.Contracts.GettingStarted>> context)
        {
            for (int i = 0; i < context.Message.Length; i++)
            {
                Console.WriteLine($"message {i}: {context.Message[i].Message.Value} ");
            }
        }
    }
}
