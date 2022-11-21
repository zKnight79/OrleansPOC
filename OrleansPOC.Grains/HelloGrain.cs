using OrleansPOC.GrainInterfaces;

namespace OrleansPOC.Grains;

public class HelloGrain : Grain, IHello
{
    public Task<string> SayHello(string name)
    {
        string greetings = $"Hello, {name} !";

        return Task.FromResult(greetings);
    }
}
