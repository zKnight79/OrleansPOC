using OrleansPOC.ClientWorker;

await Host.CreateDefaultBuilder(args)
    .UseOrleansClient((ctx, clientBuilder) =>
    {
        string? mongoCS = ctx.Configuration["MongoProvider:ConnectionString"];
        string? mongoDN = ctx.Configuration["MongoProvider:DatabaseName"];
        if (string.IsNullOrWhiteSpace(mongoCS) || string.IsNullOrWhiteSpace(mongoDN))
        {
            clientBuilder.UseLocalhostClustering();
        }
        else
        {
            clientBuilder.UseMongoDBClient(mongoCS);
            clientBuilder.UseMongoDBClustering(options =>
            {
                options.DatabaseName = mongoDN;
            });
        }
    })
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.Configure<ConsoleLifetimeOptions>(options => options.SuppressStatusMessages = true);
    })
    .RunConsoleAsync();
