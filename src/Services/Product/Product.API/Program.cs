using Common.Loggin;
using Product.API.Extensions;
using Product.API.Presistance;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(Serilogger.Configure);

Log.Information("Start Product API up");
// Add services to the container.

try
{

    builder.Host.UseSerilog(Serilogger.Configure);
    builder.Host.AddAppConfiguration();
    // add service to the container.
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
catch( Exception ex)
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