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

            })
            .Build();

            await host.RunAsync();
        }
    }
}

