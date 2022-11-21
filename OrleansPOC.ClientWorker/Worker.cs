using OrleansPOC.GrainInterfaces;

namespace OrleansPOC.ClientWorker
{
    public class Worker : BackgroundService
    {
        private const string QUIT_CMD = "/quit";
        private const string HELP_CMD = "/help";
        private const string HELLO_CMD = "/hello";

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
            Console.WriteLine($"- `{QUIT_CMD}` to exit");
            Console.WriteLine($"- `{HELP_CMD}` to show help");
            Console.WriteLine($"- `{HELLO_CMD} <name>` to use the hello grain");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(1000, stoppingToken);
            
            ShowHelp();

            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("Enter your command :");
                string? cmd = Console.ReadLine();
                if (cmd is null or QUIT_CMD)
                {
                    _applicationLifetime.StopApplication();
                    return;
                }
                else if (cmd is HELP_CMD)
                {
                    ShowHelp();
                }
                else if (cmd.StartsWith(HELLO_CMD))
                {
                    IHello helloGrain = _clusterClient.GetGrain<IHello>(Guid.NewGuid());
                    string name = cmd.Replace(HELLO_CMD, "").Trim();
                    string hello = await helloGrain.SayHello(name);
                    Console.WriteLine($"[{helloGrain.GetPrimaryKey()}] said `{hello}`");
                }
                else
                {
                    Console.WriteLine("Sorry, I didn't understand ...");
                }
            }
        }
    }
}