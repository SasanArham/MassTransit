using GettingStarted;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace WorkerProject
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = CreateHostBuilder(args).Build();
            await builder.RunAsync();
        }
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {

                var connectionString = "Server=.;Database=MassTransitDb;User Id=sa;Password=123!@#qweQWE;TrustServerCertificate=Yes;";
                services.AddDbContext<DatabaseContext>(
                    (sp) =>
                    {
                        sp.UseLazyLoadingProxies();
                        sp.UseSqlServer(connectionString, b => b.MigrationsAssembly(typeof(DatabaseContext).Assembly.FullName));
                    });
                services.AddScoped<IDatabaseContext>(provider => provider.GetRequiredService<DatabaseContext>());

                services.AddMassTransit(x =>
                {

                    x.SetKebabCaseEndpointNameFormatter();


                    var entryAssembly = Assembly.GetEntryAssembly();

                    x.AddConsumers(entryAssembly);
                    x.AddSagaStateMachines(entryAssembly);
                    x.AddSagas(entryAssembly);
                    x.AddActivities(entryAssembly);

                    x.AddEntityFrameworkOutbox<DatabaseContext>(o =>
                    {
                        o.QueryDelay = TimeSpan.FromSeconds(10);
                        o.UseSqlServer();
                        o.UseBusOutbox();
                    });


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
