namespace Linn.Purchasing.Service.Extensions
{
    using System;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Routing;

    // todo - move to Common
    public static class EndpointRouteBuilderExtensions
    {
        private static readonly string[] PatchVerb = new[] { "PATCH" };

        // looks like dotnet have copied us and added this one?

        // public static RouteHandlerBuilder MapPatch(
        //     this IEndpointRouteBuilder endpoints,
        //     string pattern,
        //     Delegate requestDelegate)
        // {
        //     return endpoints.MapMethods(pattern, PatchVerb, requestDelegate);
        // }
    }
}
