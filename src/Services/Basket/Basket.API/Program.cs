using Basket.API;
using Basket.API.Extensions;
using Common.Loggin;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();


var builder = WebApplication.CreateBuilder(args);

Log.Information( $"Start {builder.Environment.ApplicationName} API up");
// Add services to the container.

try
{

    // add service container
    builder.Host.UseSerilog(Serilogger.Configure);
    builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
    builder.Host.AddAppConfiguration();
    builder.Services.AddConfigurationSettings(builder.Configuration);

    builder.Services.ConfigureServices();
    builder.Services.ConfigRedis(builder.Configuration);
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // register service
    builder.Services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));
    // Configure Mass Transit
    builder.Services.ConfigMassTransit();


    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    //app.UseHttpsRedirection(); only for production

    app.UseAuthorization();

    app.MapControllers();

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
    Log.Information("Shut down Basket API complete");
    Log.CloseAndFlush();
}