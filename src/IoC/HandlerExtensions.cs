namespace Linn.Purchasing.IoC
{
    using System.Collections.Generic;

    using Linn.Common.Facade.Carter;
    using Linn.Common.Facade.Carter.Handlers;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Service.ResultHandlers;

    using Microsoft.Extensions.DependencyInjection;

    public static class HandlerExtensions
    {
        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            return services.AddTransient<UniversalResponseNegotiator>()
                .AddTransient<IHandler, ThingResourceResultHandler>()
                .AddTransient<IHandler, JsonResultHandler<PartSupplierResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<PartSupplierResource>>>()
                .AddTransient<IHandler, JsonResultHandler<SupplierResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<SupplierResource>>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<ThingResource>>>()
                .AddTransient<IHandler, JsonResultHandler<ProcessResultResource>>()
                .AddTransient<IHandler, JsonResultHandler<SigningLimitResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<SigningLimitResource>>>()
                .AddTransient<IHandler, SigningLimitApplicationStateResultHandler>()
                .AddTransient<IHandler, PartSupplierApplicationStateResultHandler>();
        }
    }
}
