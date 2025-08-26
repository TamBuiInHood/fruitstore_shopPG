using Common.Loggin;
using Inventory.Product.API;
using Inventory.Product.API.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(Serilogger.Configure);

Log.Information($"Start {builder.Environment.ApplicationName} up");
// Add services to the container.

try
{

    // add service container
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
    builder.Services.AddConfigurationSettings(builder.Configuration);
    builder.Services.AddInfrastructureServices();
    builder.Services.ConfigureMongoDbClient();
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    
    //app.UseHttpsRedirection(); for production

    app.UseAuthorization();

    app.MapDefaultControllerRoute();

    app.MigrateDatabase()
        .Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal)) throw;
    Console.WriteLine("Startup failed: " + ex);
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down Ordering API complete");
    Log.CloseAndFlush();
}