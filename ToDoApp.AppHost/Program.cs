using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres");

if (builder.Environment.IsDevelopment())
{
    postgres = postgres.WithPgAdmin();
}

var postgresDb = postgres.AddDatabase("postgresdb");

var apiService = builder.AddProject<Projects.ToDoApp_ApiService>("api")
    .WithReference(postgresDb)
    .WaitFor(postgres);

builder.AddProject<Projects.ToDoApp_Web>("web")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
