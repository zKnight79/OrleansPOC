using Microsoft.Extensions.Logging;
using OrleansPOC.GrainInterfaces;

namespace OrleansPOC.Grains;

public class HelloGrain : LoggableGrain<HelloGrain>, IHello
{
    public HelloGrain(ILogger<HelloGrain> logger)
        : base(logger)
    {
    }

    public Task<string> SayHello(string name)
    {
        Log($"SayHello({name})");
        
        string greetings = $"Hello, {name} !";

        return Task.FromResult(greetings);
    }
}
