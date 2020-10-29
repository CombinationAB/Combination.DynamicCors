using System;
using Combination.DynamicCors;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

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
    }
}
