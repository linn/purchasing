namespace Linn.Purchasing.IoC
{
    using System.Collections.Generic;

    using Linn.Common.Facade.Carter;
    using Linn.Common.Facade.Carter.Handlers;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Service.ResultHandlers;

    using Microsoft.Extensions.DependencyInjection;

    public static class HandlerExtensions
    {
        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            return services.AddTransient<UniversalResponseNegotiator>()
                .AddTransient<IHandler, JsonResultHandler<PartSupplierResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<PartSupplierResource>>>()
                .AddTransient<IHandler, JsonResultHandler<SupplierResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<SupplierResource>>>()
                .AddTransient<IHandler, JsonResultHandler<ReportReturnResource>>()
                .AddTransient<IHandler, JsonResultHandler<ProcessResultResource>>()
                .AddTransient<IHandler, JsonResultHandler<SigningLimitResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<SigningLimitResource>>>()
                .AddTransient<IHandler, SigningLimitApplicationStateResultHandler>()
                .AddTransient<IHandler, PartSupplierApplicationStateResultHandler>()
                .AddTransient<IHandler, JsonResultHandler<CurrencyResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<CurrencyResource>>>()
                .AddTransient<IHandler, JsonResultHandler<OrderMethodResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<OrderMethodResource>>>()
                .AddTransient<IHandler, JsonResultHandler<LinnDeliveryAddressResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<LinnDeliveryAddressResource>>>()
                .AddTransient<IHandler, JsonResultHandler<UnitOfMeasureResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<UnitOfMeasureResource>>>()
                .AddTransient<IHandler, JsonResultHandler<PackagingGroupResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<PackagingGroupResource>>>()
                .AddTransient<IHandler, JsonResultHandler<TariffResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<TariffResource>>>()
                .AddTransient<IHandler, JsonResultHandler<ManufacturerResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<ManufacturerResource>>>()
                .AddTransient<IHandler, JsonResultHandler<ReportReturnResource>>()
                .AddTransient<IHandler, JsonResultHandler<PreferredSupplierChangeResource>>()
                .AddTransient<IHandler, JsonResultHandler<PriceChangeReasonResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<PriceChangeReasonResource>>>();
        }
    }
}
