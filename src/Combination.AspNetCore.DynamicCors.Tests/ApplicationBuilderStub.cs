using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Combination.DynamicCors.Tests
{
    internal sealed class ApplicationBuilderStub : IApplicationBuilder
    {
        public ApplicationBuilderStub()
        {
            var sb = new ServiceCollection();
            sb.AddSingleton<ILoggerFactory, NullLoggerFactory>();
            ApplicationServices = sb.BuildServiceProvider();
        }

        public IServiceProvider ApplicationServices { get; set; }

        public IDictionary<string, object> Properties => throw new NotImplementedException();

        public IFeatureCollection ServerFeatures => throw new NotImplementedException();

        public RequestDelegate Build()
        {
            throw new NotImplementedException();
        }

        public IApplicationBuilder New()
        {
            throw new NotImplementedException();
        }


        public int MiddlewareCount { get; private set; }
        public IApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware)
        {
            MiddlewareCount++;
            return this;
        }
    }
}
