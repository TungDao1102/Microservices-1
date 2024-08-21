using Order.API;
using Order.Application;
using Order.Infrastructure;
using Order.Infrastructure.Data.Extentions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplicationServices(builder.Configuration)
                .AddInfrastructureServices(builder.Configuration)
                .AddApiServices(builder.Configuration);

var app = builder.Build();

app.UseApiServices();

if (app.Environment.IsDevelopment())
{
    await app.InitialDatabaseAsync();
}

app.MapGet("/", () => "Hello World!");

app.Run();
