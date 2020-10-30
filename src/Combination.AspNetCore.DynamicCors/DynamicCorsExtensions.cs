using System;
using Combination.DynamicCors;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing;

namespace Combination
{
    public static class DynamicCorsExtensions
    {
        public static IApplicationBuilder UseDynamicCors(this IApplicationBuilder applicationBuilder, Action<IServiceProvider, IDynamicCorsFactory> factory)
        {
            var fac = new DynamicCorsFactory();
            factory(applicationBuilder.ApplicationServices, fac);
            return UseDynamicCors(applicationBuilder, fac);
        }

        public static IApplicationBuilder UseDynamicCors(this IApplicationBuilder applicationBuilder, Action<IDynamicCorsFactory> factory)
        {
            var fac = new DynamicCorsFactory();
            factory(fac);
            return UseDynamicCors(applicationBuilder, fac);
        }

        public static IEndpointRouteBuilder MapDynamicCors(this IEndpointRouteBuilder endpointRouteBuilder, string pattern, Action<IDynamicCorsFactory> factory)
        {
            var fac = new DynamicCorsFactory();
            factory(fac);
            return MapDynamicCors(pattern, endpointRouteBuilder, fac);
        }

        public static IEndpointRouteBuilder MapDynamicCors(this IEndpointRouteBuilder endpointRouteBuilder, string pattern, Action<IServiceProvider, IDynamicCorsFactory> factory)
        {
            var fac = new DynamicCorsFactory();
            factory(endpointRouteBuilder.ServiceProvider, fac);
            return MapDynamicCors(pattern, endpointRouteBuilder, fac);
        }

        private static IApplicationBuilder UseDynamicCors(IApplicationBuilder applicationBuilder, DynamicCorsFactory factory)
        {
            var logger = applicationBuilder.ApplicationServices.GetService<ILogger<DynamicCorsMiddleware>>();

            if (factory.Pattern == null)
            {
                logger?.LogInformation("No CORS pattern defined. Will not enable CORS middleware.");
                return applicationBuilder;
            }

            var corsMiddleware = new DynamicCorsMiddleware(factory.Pattern, string.Join(", ", factory.Methods), logger);
            applicationBuilder.Use(corsMiddleware.Invoke);
            return applicationBuilder;
        }

        private static IEndpointRouteBuilder MapDynamicCors(string pattern, IEndpointRouteBuilder endpointRouteBuilder, DynamicCorsFactory factory)
        {
            var logger = endpointRouteBuilder.ServiceProvider.GetService<ILogger<DynamicCorsMiddleware>>();

            if (factory.Pattern == null)
            {
                logger?.LogInformation("No CORS pattern defined. Will not enable CORS middleware.");
                return endpointRouteBuilder;
            }

            var corsMiddleware = new DynamicCorsMiddleware(factory.Pattern, string.Join(", ", factory.Methods), logger);
            endpointRouteBuilder.MapMethods(pattern, new[] { "OPTIONS" }, corsMiddleware.Invoke);
            return endpointRouteBuilder;
        }
    }
}
