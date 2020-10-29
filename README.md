# Combination.AspNetCore.DynamicCors

Dynamic CORS middleware based on Regular Expressions for ASP.NET Core

Usage

    applicationBuilder.UseDynamicCors(factory => factory.WithPatterns(@".+\.mydomain\.(com|net|org)").WithMethods("POST"));


The middleware will dynamically look at each request and generate a correct CORS origin header if the origin matches, otherwise no headers
is sent (The Vary header is set to prevent caching of invalid repsonses). This allows for flexibility in dealing with complex Origin
policies, while not leaking the full set of origins to the requester.
