using GettingStarted;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using System.Threading.Tasks;

namespace WorkerProject
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }


        // for in memory: 
        //public static IHostBuilder CreateHostBuilder(string[] args)
        //{
        //    Host.CreateDefaultBuilder(args)
        //    .ConfigureServices((hostContext, services) =>
        //    {
        //        services.AddMassTransit(x =>
        //        {
        //            x.SetKebabCaseEndpointNameFormatter();

        //            // By default, sagas are in-memory, but should be changed to a durable
        //            // saga repository.
        //            x.SetInMemorySagaRepositoryProvider();

        //            var entryAssembly = Assembly.GetEntryAssembly();

        //            x.AddConsumers(entryAssembly);
        //            x.AddSagaStateMachines(entryAssembly);
        //            x.AddSagas(entryAssembly);
        //            x.AddActivities(entryAssembly);

        //            x.UsingInMemory((context, cfg) =>
        //            {
        //                cfg.ConfigureEndpoints(context);
        //            });
        //        });

        //        services.AddHostedService<Worker>();

        //    });
        //}




        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddMassTransit(x =>
                {

                    x.SetKebabCaseEndpointNameFormatter();


                    var entryAssembly = Assembly.GetEntryAssembly();

                    x.AddConsumers(entryAssembly);
                    x.AddSagaStateMachines(entryAssembly);
                    x.AddSagas(entryAssembly);
                    x.AddActivities(entryAssembly);

                    x.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.Host("localhost", "/", h =>
                        {
                            h.Username("guest");
                            h.Password("guest");
                        });
                        cfg.ConfigureEndpoints(context);
                    });
                });
                services.AddHostedService<Worker>();
            });
        }

    }
}
