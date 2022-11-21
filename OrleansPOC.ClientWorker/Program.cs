using OrleansPOC.ClientWorker;

await Host.CreateDefaultBuilder(args)
    .UseOrleansClient(clientBuilder =>
    {
        clientBuilder.UseLocalhostClustering();
    })
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.Configure<ConsoleLifetimeOptions>(options => options.SuppressStatusMessages= true);
    })
    .RunConsoleAsync();
