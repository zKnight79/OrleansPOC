using Orleans.Configuration;

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
                siloBuilder.Configure<ClusterMembershipOptions>(options =>
                {
                    options.DefunctSiloCleanupPeriod = TimeSpan.FromSeconds(30);
                    options.DefunctSiloExpiration = TimeSpan.FromSeconds(30);
                    options.TableRefreshTimeout = TimeSpan.FromMinutes(5);
                });
            }
            else
            {
                siloBuilder.UseLocalhostClustering();
            }
            siloBuilder.AddMongoDBGrainStorageAsDefault(options =>
            {
                options.DatabaseName = mongoDN;
            });
        }
    })
    .RunConsoleAsync();
