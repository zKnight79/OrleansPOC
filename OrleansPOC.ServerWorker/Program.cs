await Host.CreateDefaultBuilder(args)
    .UseOrleans((ctx, siloBuilder) =>
    {
        string? mongoCS = ctx.Configuration["MongoProvider:ConnectionString"];
        string? mongoDN = ctx.Configuration["MongoProvider:DatabaseName"];
        bool mongoClustering = ctx.Configuration.GetValue<bool>("MongoProvider:UseClustering");
        if (string.IsNullOrWhiteSpace(mongoCS) || string.IsNullOrWhiteSpace(mongoDN))
        {
            siloBuilder.UseLocalhostClustering();
            siloBuilder.AddMemoryGrainStorageAsDefault();
        }
        else
        {
            siloBuilder.UseMongoDBClient(mongoCS);
            if (mongoClustering)
            {
                siloBuilder.UseMongoDBClustering(options =>
                {
                    options.DatabaseName = mongoDN;
                });
            }
            else
            {
                siloBuilder.UseLocalhostClustering();
            }
            siloBuilder.AddMongoDBGrainStorageAsDefault(options =>
            {
                options.DatabaseName= mongoDN;
            });
        }
    })
    .RunConsoleAsync();
