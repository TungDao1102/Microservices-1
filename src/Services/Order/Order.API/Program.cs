using Order.Infrastructure;
using Order.Application;
using Order.API;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplicationServices(builder.Configuration)
                .AddInfrastructureServices(builder.Configuration)
                .AddApiServices(builder.Configuration);

var app = builder.Build();

app.UseApiServices();

app.MapGet("/", () => "Hello World!");

app.Run();
