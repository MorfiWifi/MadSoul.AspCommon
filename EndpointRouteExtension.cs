using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MadSoul.AspCommon;

public static class EndpointRouteExtension
{
    /// <summary>
    /// add endpoints as singletons to services
    /// </summary>
    /// <param name="services">services collection</param>
    /// <param name="assembly"></param>
    public static void AddEndpointsFromAssembly (this IServiceCollection services  , Assembly assembly)
    {
        var endpoints = assembly.GetTypes()
            .Where(x=> x.IsClass && typeof(IEndpointRoute)
                .IsAssignableFrom(x))
            .ToList();

        foreach (var endpoint in endpoints)
        {
            services.AddSingleton(typeof(IEndpointRoute), endpoint);
        }
    }

    /// <summary>
    /// register endpoints as routes for http server
    /// </summary>
    /// <param name="app">web application / webapi</param>
    public static void RegisterEndpoints(this WebApplication app)
    {
       var endpoints = app.Services.GetServices<IEndpointRoute>();
       foreach (var endpoint in endpoints)
       {
           endpoint.RegisterRoute(app);
       }
    }
    
}