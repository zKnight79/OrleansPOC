await Host.CreateDefaultBuilder(args)
    .UseOrleans((ctx, siloBuilder) =>
    {
        siloBuilder.UseLocalhostClustering();

        string? mongoCS = ctx.Configuration["MongoProvider:ConnectionString"];
        string? mongoDN = ctx.Configuration["MongoProvider:DatabaseName"];
        if (string.IsNullOrWhiteSpace(mongoCS) || string.IsNullOrWhiteSpace(mongoDN))
        {
            siloBuilder.AddMemoryGrainStorageAsDefault();
        }
        else
        {
            siloBuilder.UseMongoDBClient(mongoCS);
            siloBuilder.AddMongoDBGrainStorageAsDefault(options =>
            {
                options.DatabaseName= mongoDN;
            });
        }
    })
    .RunConsoleAsync();
