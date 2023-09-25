# Combination.AspNetCore.DynamicCors

Dynamic CORS middleware based on Regular Expressions for ASP.NET Core

Usage

    applicationBuilder.UseDynamicCors(factory => factory.WithPatterns(@".+\.mydomain\.(com|net|org)").WithMethods("POST"));

The middleware will dynamically look at each request and generate a correct CORS origin header if the origin matches, otherwise no headers
is sent (The Vary header is set to prevent caching of invalid repsonses). This allows for flexibility in dealing with complex Origin
policies, while not leaking the full set of origins to the requester.

## ServiceBase

To use Dynamic CORS with [ServiceBase](https://github.com/Sports-Global/Sports-Framework-ServiceBase), make sure to call UseDynamicCors first in the middleware pipeline.

```
var builder = ServiceApplication.CreateBuilder(args, options =>
{
    options.Logging.Clear(DefaultLoggingStrategy.DefaultWithRequests);
    options.ApplicationBuilder.Configure(app =>
    {
        app.UseDynamicCors(cors =>
            cors.WithPattern(allowedCorsHosts));
    });
});
```
