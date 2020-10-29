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
        private readonly ILogger logger;

        public DynamicCorsMiddleware(Regex allowedHosts, string methods, ILogger<DynamicCorsMiddleware> logger)
        {
            this.allowedHosts = allowedHosts;
            this.methods = methods;
            this.logger = logger;
        }

        public Task Invoke(HttpContext context, Func<Task> next)
        {
            var origin = string.Empty;
            var isReferer = false;

            if (context.Request.Headers.TryGetValue("Origin", out var value))
            {
                var valueStr = value.FirstOrDefault();
                if (!string.Equals(valueStr, "null", StringComparison.OrdinalIgnoreCase))
                {
                    origin = valueStr;
                }
            }

            // Origin will be "null" for redirects
            if (string.IsNullOrEmpty(origin) && context.Request.Headers.TryGetValue("Referer", out var referer))
            {
                var valueStr = referer.FirstOrDefault();
                var index = valueStr.IndexOf("//", StringComparison.InvariantCultureIgnoreCase) + 2;
                index = valueStr.IndexOf('/', index);

                if (index > 0)
                {
                    valueStr = valueStr.Substring(0, index);
                }

                origin = valueStr;
                isReferer = true;
            }

            if (!string.IsNullOrEmpty(origin) && allowedHosts.IsMatch(origin))
            {
                context.Response.Headers.Add("Access-Control-Allow-Origin", isReferer ? "null" : origin);
                context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Content-Length");
                context.Response.Headers.Add("Access-Control-Allow-Methods", methods);
                context.Response.Headers.Add("Access-Control-Max-Age", "86400");
                context.Response.Headers.Add("Vary", "Origin");
            }
            else if (!string.IsNullOrEmpty(origin))
            {
                logger.LogWarning("CORS validation failed for origin: {0}", origin);
            }

            if (context.Request.Method == HttpMethods.Options)
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                return Task.CompletedTask;
            }
            else
            {
                return next.Invoke();
            }
        }
    }
}
