var builder = DistributedApplication.CreateBuilder(args);

var bff = builder.AddProject<Projects.NSE_Bff_Compras>("nse-compras-bff");

builder.AddProject<Projects.NSE_WebApp_MVC>("nse-webapp-mvc")
    .WithReference(bff);


builder.Build().Run();
