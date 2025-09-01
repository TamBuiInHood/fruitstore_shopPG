using Common.Loggin;
using Infrastructure.Middlewares;
using Ocelot.Middleware;
using OcelotApiGw.Extensions;
using Serilog;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();
var builder = WebApplication.CreateBuilder(args);

Log.Information($"Start {builder.Environment.ApplicationName} up");
// Add services to the container.

try
{
    // Add services to the container.
    builder.Host.AddAppConfigurations();
    builder.Services.AddControllers();
    builder.Services.AddConfigurationSettings(builder.Configuration);
    builder.Services.ConfigureOcelot(builder.Configuration);
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.ConfigureCors(builder.Configuration);
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json",
                "Swagger Customer Minimal API v1");
        });
    }

    //app.UseHttpsRedirection();
    app.UseCors("CorsPolicy");
    app.UseMiddleware<ErrorWrappingMiddlewares>();
    app.UseAuthorization();

    app.MapControllers();
    await app.UseOcelot();
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
    Log.Information($"Shut down {builder.Environment.ApplicationName}");
    Log.CloseAndFlush();
}