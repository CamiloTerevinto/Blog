var builder = DistributedApplication.CreateBuilder(args);

var pgsql = builder.AddPostgres("postgresql")
    .WithDataVolume("data")
    .WithPgAdmin();

var shopDatabase = pgsql.AddDatabase("Shop", "shop");

builder.AddProject<Projects.CT_Examples_CqrsWithAspire_Command_Api>("CommandApi")
    .WithReference(shopDatabase);

builder.AddProject<Projects.CT_Examples_CqrsWithAspire_Query_Api>("QueryApi")
    .WithReference(shopDatabase);

builder.Build().Run();
