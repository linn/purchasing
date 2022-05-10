namespace Linn.Purchasing.Service.Extensions
{
    using System;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Routing;

    public static class EndpointRouteBuilderExtensions
    {
        private static readonly string[] PatchVerb = new[] { "PATCH" };

        public static RouteHandlerBuilder MapPatch(
            this IEndpointRouteBuilder endpoints,
            string pattern,
            Delegate requestDelegate)
        {
            return endpoints.MapMethods(pattern, PatchVerb, requestDelegate);
        }
    }
}
