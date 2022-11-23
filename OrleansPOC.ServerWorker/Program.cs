await Host.CreateDefaultBuilder(args)
    .UseOrleans(siloBuilder =>
    {
        siloBuilder.UseLocalhostClustering();
        siloBuilder.AddMemoryGrainStorageAsDefault();
        
    })
    .RunConsoleAsync();
