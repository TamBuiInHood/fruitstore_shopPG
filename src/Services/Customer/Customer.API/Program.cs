using Common.Loggin;
using Contracts.Common.Interfaces;
using Customer.API;
using Customer.API.Controller;
using Customer.API.Extensions;
using Customer.API.Persistence;
using Customer.API.Repositories;
using Customer.API.Repositories.Interfaces;
using Customer.API.Services;
using Customer.API.Services.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using Shared.DTOs.CustomerDTO;

var builder = WebApplication.CreateBuilder(args);

Log.Information($"Start {builder.Environment.EnvironmentName} up");
// Add services to the container.

try
{

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.ConfigureServices();
    builder.Services.AddInfrastructureServices(builder.Configuration);
    builder.Services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));

    var app = builder.Build();

    // MINIMAL API
    app.MapGet("/", handler: () => "Welcome to Customer API!");

    app.MapCustomerAPIs();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json",
                "Swagger Customer Minimal API v1");
        });
    }

    //app.UseHttpsRedirection();    //production only
    app.UseAuthorization();
    app.MapControllers();
    app.SeedCustomerData()
        .Run();
}
catch (Exception ex)
{
    // catch exception roi log ra 
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal)) throw;

    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    // khi end ct thi log ra de biet luc dung log luon
    Log.Information("Shut down Customer API complete");
    Log.CloseAndFlush();
}