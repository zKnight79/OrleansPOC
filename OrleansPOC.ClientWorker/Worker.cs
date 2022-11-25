using OrleansPOC.GrainInterfaces;
using System.Text.RegularExpressions;

namespace OrleansPOC.ClientWorker
{
    public partial class Worker : BackgroundService
    {
        private const string QUIT_CMD = "/quit";
        private const string HELP_CMD = "/help";
        private const string HELLO_CMD = "/hello";
        private const string PRODUCT_CMD = "/prod";
        private const string NAME_ARG = "name";
        private const string CODE_ARG = "code";

        private readonly IClusterClient _clusterClient;
        private readonly IHostApplicationLifetime _applicationLifetime;

        public Worker(IClusterClient clusterClient, IHostApplicationLifetime applicationLifetime)
        {
            _clusterClient = clusterClient;
            _applicationLifetime = applicationLifetime;
        }

        [GeneratedRegex($"{QUIT_CMD}")]
        private static partial Regex QuitCommandRegex();

        [GeneratedRegex($"{HELP_CMD}")]
        private static partial Regex HelpCommandRegex();

        [GeneratedRegex(@$"{HELLO_CMD} (?<{NAME_ARG}>.+)")]
        private static partial Regex HelloCommandRegex();

        [GeneratedRegex(@$"{PRODUCT_CMD} (?<{CODE_ARG}>\w+) (?<{NAME_ARG}>.+)")]
        private static partial Regex ProductCommandRegex();

        private static bool ParseQuitCommand(string cmd)
        {
            Match match = QuitCommandRegex().Match(cmd);
            return match.Success;
        }
        private static bool ParseHelpCommand(string cmd)
        {
            Match match = HelpCommandRegex().Match(cmd);
            return match.Success;
        }
        private static bool ParseHelloCommand(string cmd, out string name)
        {
            bool result = false;
            
            if (HelloCommandRegex().Match(cmd) is { Success: true } matchHello)
            {
                name = matchHello.Groups[NAME_ARG].Value;
                result = true;
            }
            else
            {
                name = "";
            }

            return result;
        }
        private static bool ParseProductCommand(string cmd, out string code, out string name)
        {
            bool result = false;

            if (ProductCommandRegex().Match(cmd) is { Success: true } matchProd)
            {
                code = matchProd.Groups[CODE_ARG].Value;
                name = matchProd.Groups[NAME_ARG].Value;
                result = true;
            }
            else
            {
                code = "";
                name = "";
            }

            return result;
        }

        private static void ShowHelp()
        {
            Console.WriteLine("Commands :");
            Console.WriteLine($"- `{QUIT_CMD}` to exit");
            Console.WriteLine($"- `{HELP_CMD}` to show help");
            Console.WriteLine($"- `{HELLO_CMD} <name>` to use the hello grain");
            Console.WriteLine($"- `{PRODUCT_CMD} <ref> <name>` to use the product grain");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(1000, stoppingToken);
            
            ShowHelp();

            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("Enter your command :");
                string? cmd = Console.ReadLine();
                if (cmd is null || ParseQuitCommand(cmd))
                {
                    _applicationLifetime.StopApplication();
                    return;
                }
                else if (ParseHelpCommand(cmd))
                {
                    ShowHelp();
                }
                else if (ParseHelloCommand(cmd, out string helloName))
                {
                    IHello helloGrain = _clusterClient.GetGrain<IHello>(Guid.NewGuid());
                    string hello = await helloGrain.SayHello(helloName);
                    Console.WriteLine($"[{helloGrain.GetPrimaryKey()}] said `{hello}`");
                }
                else if (ParseProductCommand(cmd, out string prodCode, out string prodName))
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