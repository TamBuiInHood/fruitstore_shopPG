using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;
using Ordering.Application.Common.Behavious;
namespace Ordering.Application.Features
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
           return services.AddAutoMapper(Assembly.GetExecutingAssembly())
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
                .AddMediatR(Assembly.GetExecutingAssembly())
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviours<,>))
                ;
        }
    }
}
