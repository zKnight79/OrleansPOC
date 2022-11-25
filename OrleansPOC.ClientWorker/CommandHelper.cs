using System.Text.RegularExpressions;
namespace OrleansPOC.ClientWorker;

internal static partial class CommandHelper
{
    private const string QUIT_CMD = "/quit";
    private const string HELP_CMD = "/help";
    private const string HELLO_CMD = "/hello";
    private const string PRODUCT_CMD = "/prod";
    private const string NAME_ARG = "name";
    private const string CODE_ARG = "code";

    public const string QUIT_HELP_SYNTAX = $"`{QUIT_CMD}` to exit";
    public const string HELP_HELP_SYNTAX = $"`{HELP_CMD}` to show help";
    public const string HELLO_HELP_SYNTAX = $"`{HELLO_CMD} <{NAME_ARG}>` to use the Hello Grain";
    public const string PRODUCT_HELP_SYNTAX = $"`{PRODUCT_CMD} <{CODE_ARG}> <{NAME_ARG}>` to use the Product Grain";

    [GeneratedRegex($"{QUIT_CMD}")] private static partial Regex QuitCommandRegex();
    [GeneratedRegex($"{HELP_CMD}")] private static partial Regex HelpCommandRegex();
    [GeneratedRegex(@$"{HELLO_CMD} (?<{NAME_ARG}>.+)")] private static partial Regex HelloCommandRegex();
    [GeneratedRegex(@$"{PRODUCT_CMD} (?<{CODE_ARG}>\w+) (?<{NAME_ARG}>.+)")] private static partial Regex ProductCommandRegex();

    public static bool ParseQuitCommand(this string cmd)
    {
        Match match = CommandHelper.QuitCommandRegex().Match(cmd);
        return match.Success;
    }
    public static bool ParseHelloCommand(this string cmd, out string name)
    {
        bool result = false;

        if (CommandHelper.HelloCommandRegex().Match(cmd) is { Success: true } matchHello)
        {
            name = matchHello.Groups[CommandHelper.NAME_ARG].Value;
            result = true;
        }
        else
        {
            name = "";
        }

        return result;
    }
    public static bool ParseHelpCommand(this string cmd)
    {
        Match match = CommandHelper.HelpCommandRegex().Match(cmd);
        return match.Success;
    }
    public static bool ParseProductCommand(this string cmd, out string code, out string name)
    {
        bool result = false;

        if (CommandHelper.ProductCommandRegex().Match(cmd) is { Success: true } matchProd)
        {
            code = matchProd.Groups[CommandHelper.CODE_ARG].Value;
            name = matchProd.Groups[CommandHelper.NAME_ARG].Value;
            result = true;
        }
        else
        {
            code = "";
            name = "";
        }

        return result;
    }
}
