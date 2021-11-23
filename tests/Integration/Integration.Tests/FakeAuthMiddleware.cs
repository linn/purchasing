namespace Linn.Purchasing.Integration.Tests
{
    using System.Security.Claims;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Http;

    public static class FakeAuthMiddleware
    {
        public static RequestDelegate EmployeeMiddleware(RequestDelegate next) => ctx =>
            {
                var identity = new ClaimsIdentity(
                    new[]
                        {
                            new Claim("sid", "12345"),
                    new Claim("employee", "/employees/1")
                },
                CookieAuthenticationDefaults.AuthenticationScheme);

            ctx.User = new ClaimsPrincipal(identity);

            return next(ctx);
        };
    }
}
