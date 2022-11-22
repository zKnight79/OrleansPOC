using Microsoft.Extensions.Logging;

namespace OrleansPOC.Grains;

public abstract class LoggableGrain<T> : Grain where T : LoggableGrain<T>
{
    private readonly ILogger<T> _logger;

    private static string GrainType { get; } = typeof(T).Name;

    protected LoggableGrain(ILogger<T> logger)
    {
        _logger = logger;
    }

    protected void Log(string message)
    {
        _logger.LogInformation("[{GrainType} / {IdentityString}] `{message}`", GrainType, IdentityString, message);
    }
}
