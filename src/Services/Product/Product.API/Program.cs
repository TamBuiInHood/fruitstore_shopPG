using Common.Loggin;
using Product.API.Extensions;
using Product.API.Presistance;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Information($"Start {builder.Environment.EnvironmentName} up");
// Add services to the container.

try
{
    builder.Host.AddAppConfiguration();
    // add service to the container.
    builder.Services.AddConfigurationSettings(builder.Configuration);
    builder.Services.AddInfracstructure(builder.Configuration);
    var app = builder.Build();

    app.UseInfracstructure();

    app.MigrateDatabase<ProductContext>((context, _) =>
    {
        ProductContextSeed.SeedProductAsync(context, Log.Logger).Wait();
    })
        .Run();
    app.Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        throw;
    }
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down Product API complete");
    Log.CloseAndFlush();
}