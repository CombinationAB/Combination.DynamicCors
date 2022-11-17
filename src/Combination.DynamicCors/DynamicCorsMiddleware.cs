using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Combination.DynamicCors
{
    internal sealed class DynamicCorsMiddleware
    {
        private readonly Regex allowedHosts;
        private readonly string methods;
        private readonly string headers;
        private readonly string exposedHeaders;
        private readonly ILogger? logger;

        public DynamicCorsMiddleware(Regex allowedHosts, string methods, string headers, ILogger<DynamicCorsMiddleware>? logger) : this(
            allowedHosts,
            methods,
            headers,
            string.Empty,
            logger)
        {
        }

        public DynamicCorsMiddleware(Regex allowedHosts, string methods, string headers, string exposedHeaders, ILogger<DynamicCorsMiddleware>? logger)
        {
            this.allowedHosts = allowedHosts;
            this.methods = methods;
            this.headers = headers;
            this.exposedHeaders = exposedHeaders;
            this.logger = logger;
        }

        public Task Invoke(HttpContext context) => Invoke(context, null);

        public Task Invoke(HttpContext context, Func<Task>? next)
        {
            var origin = string.Empty;
            var isReferer = false;

            if (context.Request.Headers.TryGetValue("Origin", out var value)
                && value.FirstOrDefault() is string originValue)
            {
                if (!string.Equals(originValue, "null", StringComparison.OrdinalIgnoreCase))
                {
                    origin = originValue;
                }
            }

            // Origin will be "null" for redirects
            if (string.IsNullOrEmpty(origin)
                && context.Request.Headers.TryGetValue("Referer", out var referer)
                && referer.FirstOrDefault() is string refererValue)
            {
                var index = refererValue.IndexOf("//", StringComparison.InvariantCultureIgnoreCase) + 2;
                index = refererValue.IndexOf('/', index);

                if (index > 0)
                {
                    refererValue = refererValue.Substring(0, index);
                }

                origin = refererValue;
                isReferer = true;
            }

            if (!string.IsNullOrEmpty(origin) && allowedHosts.IsMatch(origin))
            {
                context.Response.Headers.Add("Access-Control-Allow-Origin", isReferer ? "null" : origin);
                context.Response.Headers.Add("Access-Control-Allow-Headers", headers);
                context.Response.Headers.Add("Access-Control-Allow-Methods", methods);
                if (!string.IsNullOrWhiteSpace(exposedHeaders))
                {
                    context.Response.Headers.Add("Access-Control-Expose-Headers", exposedHeaders);
                }

                context.Response.Headers.Add("Access-Control-Max-Age", "86400");
            }
            else if (!string.IsNullOrEmpty(origin))
            {
                logger?.LogWarning("CORS validation failed for origin: {0}. Methods: {1}. Hosts: {2}", origin, methods, allowedHosts.ToString());
            }

            // Vary: Origin is always supposed to be sent. https://stackoverflow.com/questions/25329405/why-isnt-vary-origin-response-set-on-a-cors-miss
            context.Response.Headers.Add("Vary", "Origin");

            if (context.Request.Method == HttpMethods.Options)
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                return Task.CompletedTask;
            }
            else if (next != null)
            {
                return next.Invoke();
            }
            else
            {
                return Task.CompletedTask;
            }
        }
    }
}
