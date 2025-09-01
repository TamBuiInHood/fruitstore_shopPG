using Infrastructure.Middlewares;

namespace AuthenServices.Extensions
{
    public static class ApplicationExtensions
    {
        public static void UseInfracstructure(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "authen.v1"));
            app.UseMiddleware<ErrorWrappingMiddlewares>();
            app.UseAuthentication();
            app.UseRouting();
            //app.UseHttpsRedirection();   // for production only
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
