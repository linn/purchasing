namespace Linn.Purchasing.IoC
{
    using System.Collections.Generic;
    using System.Net.Http;

    using Amazon.Extensions.NETCore.Setup;
    using Amazon.SimpleEmail;

    using Linn.Common.Configuration;
    using Linn.Common.Email;
    using Linn.Common.Facade;
    using Linn.Common.Pdf;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Facade.ResourceBuilders;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Persistence.LinnApps.Keys;
    using Linn.Purchasing.Resources;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceExtensions
    {
        public static IServiceCollection AddFacade(this IServiceCollection services)
        {
            return services.AddTransient<IBuilder<Thing>, ThingResourceBuilder>()
                .AddTransient<IFacadeResourceService<Thing, int, ThingResource, ThingResource>, ThingFacadeService>()
                .AddTransient<IBuilder<PartSupplier>, PartSupplierResourceBuilder>()
                .AddTransient<IBuilder<IEnumerable<PartSupplier>>, PartSuppliersResourceBuilder>()
                .AddTransient<IFacadeResourceService<PartSupplier, PartSupplierKey, PartSupplierResource, PartSupplierResource>, PartSupplierFacadeService>()
                .AddTransient<IPurchaseOrderReportFacadeService, PurchaseOrderReportFacadeService>();
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services.AddTransient<IThingService, ThingService>()
                .AddTransient<IAmazonSimpleEmailService>(
                    x => new AmazonSimpleEmailServiceClient(x.GetService<AWSOptions>()?.Region))
                .AddTransient<IEmailService>(x => new EmailService(x.GetService<IAmazonSimpleEmailService>()))
                .AddTransient<ITemplateEngine, TemplateEngine>().AddTransient<IPdfService>(
                    x => new PdfService(ConfigurationManager.Configuration["PDF_SERVICE_ROOT"], new HttpClient()))
                .AddTransient<IReportingHelper, ReportingHelper>()
                .AddTransient<IPurchaseOrdersReportService, PurchaseOrdersReportService>();
        }
    }
}
