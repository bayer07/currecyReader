using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CurrencyReader.Service
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddHostedService<HostedService>();
                services.AddSingleton<CnbCzService>();
                services.AddSingleton<Parser>();
                services.AddHttpClient();
            })
            .Build();

            await host.RunAsync();
        }
    }
}

