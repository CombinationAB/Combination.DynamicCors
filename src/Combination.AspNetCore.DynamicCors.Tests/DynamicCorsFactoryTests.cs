using Combination.DynamicCors.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Combination.DynamicCors.Tests
{
    public class DynamicCorsFactoryTests
    {
        [Fact]
        public void Middleware_Added()
        {
            var appBuilder = new ApplicationBuilderStub();
            appBuilder.UseDynamicCors(factory => factory.WithPattern(".+"));
            Assert.Equal(1, appBuilder.MiddlewareCount);
        }
        [Fact]
        public void Middleware_Not_Added_If_No_Patterns()
        {
            var appBuilder = new ApplicationBuilderStub();
            appBuilder.UseDynamicCors(factory => { });
            Assert.Equal(0, appBuilder.MiddlewareCount);
        }
    }
}
