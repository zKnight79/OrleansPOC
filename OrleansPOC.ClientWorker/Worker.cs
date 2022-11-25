using OrleansPOC.GrainInterfaces;
using System.Text.RegularExpressions;

namespace OrleansPOC.ClientWorker
{
    public class Worker : BackgroundService
    {
        private readonly IClusterClient _clusterClient;
        private readonly IHostApplicationLifetime _applicationLifetime;

        public Worker(IClusterClient clusterClient, IHostApplicationLifetime applicationLifetime)
        {
            _clusterClient = clusterClient;
            _applicationLifetime = applicationLifetime;
        }

        private static void ShowHelp()
        {
            Console.WriteLine("Commands :");
            foreach (string syntax in CommandHelper.GetCommandHelpSyntaxes())
            {
                Console.WriteLine($"- {syntax}");
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(1000, stoppingToken);
            
            ShowHelp();

            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("Enter your command :");
                string? cmd = Console.ReadLine();
                if (cmd is null || cmd.ParseQuitCommand())
                {
                    _applicationLifetime.StopApplication();
                    return;
                }
                else if (cmd.ParseHelpCommand())
                {
                    ShowHelp();
                }
                else if (cmd.ParseHelloCommand(out string helloName))
                {
                    IHello helloGrain = _clusterClient.GetGrain<IHello>(Guid.NewGuid());
                    string hello = await helloGrain.SayHello(helloName);
                    Console.WriteLine($"[{helloGrain.GetPrimaryKey()}] said `{hello}`");
                }
                else if (cmd.ParseProductCommand(out string prodCode, out string prodName))
                {
                    Console.WriteLine($"Try getting product `{prodCode}` to set name to `{prodName}`");
                    IProduct productGrain = _clusterClient.GetGrain<IProduct>(prodCode);
                    string oldName = await productGrain.GetName();
                    Console.WriteLine($"`{prodCode}` product name was `{oldName}`");
                    await productGrain.SetName(prodName);
                }
                else
                {
                    Console.WriteLine("Sorry, I didn't understand ...");
                }
            }
        }
    }
}