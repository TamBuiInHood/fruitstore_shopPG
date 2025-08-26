using Common.Loggin;
using Ordering.API.Extensions;
using Ordering.Application.Features;
using Ordering.Infastructure.Persistance;
using Ordering.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(Serilogger.Configure);

Log.Information("Start Ordering API up");
// Add services to the container.

try
{

    // add service container
    builder.Host.AddAppConfigurations();
    builder.Services.AddInfracstructureService(builder.Configuration);
    builder.Services.AddControllers();
    builder.Services.AddApplicationServices();
    builder.Services.AddConfigurationSettings(builder.Configuration);
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.ConfigureMassTransit();
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    // Initialies and seed database
    using (var scope = app.Services.CreateScope())
    {
        var orderContextSeed = scope.ServiceProvider.GetRequiredService<OrderContextSeed>();
        await orderContextSeed.InitialiseAsync();
        await orderContextSeed.SeedAsync();
    }
    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
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