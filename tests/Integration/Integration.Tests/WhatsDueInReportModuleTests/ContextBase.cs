namespace Linn.Purchasing.Integration.Tests.WhatsDueInReportModuleTests
{
    using System.Net.Http;

    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Facade.ResourceBuilders;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.IoC;
    using Linn.Purchasing.Service.Modules.Reports;

    using Microsoft.Extensions.DependencyInjection;

    using NSubstitute;

    using NUnit.Framework;

    public abstract class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected HttpResponseMessage Response { get; set; }

        protected IWhatsDueInReportFacadeService ReportFacadeService { get; set; }

        protected IWhatsDueInReportService MockDomainService { get; set; }

        [SetUp]
        public void EstablishContext()
        {
            this.MockDomainService = Substitute.For<IWhatsDueInReportService>();
            this.ReportFacadeService = new WhatsDueInReportFacadeService(
                this.MockDomainService, new ResultsModelResourceBuilder());

            this.Client = TestClient.With<WhatsDueInReportModule>(
                services =>
                    {
                        services.AddSingleton(this.ReportFacadeService);
                        services.AddHandlers();
                    },
                FakeAuthMiddleware.EmployeeMiddleware);
        }
    }
}
