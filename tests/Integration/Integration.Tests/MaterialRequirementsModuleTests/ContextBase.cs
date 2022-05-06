namespace Linn.Purchasing.Integration.Tests.MaterialRequirementsModuleTests
{
    using System.Net.Http;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Facade.ResourceBuilders;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.IoC;
    using Linn.Purchasing.Resources.MaterialRequirements;
    using Linn.Purchasing.Resources.SearchResources;
    using Linn.Purchasing.Service.Modules;

    using Microsoft.Extensions.DependencyInjection;

    using NSubstitute;

    using NUnit.Framework;

    public abstract class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected HttpResponseMessage Response { get; set; }

        protected IFacadeResourceFilterService<MrpRunLog, int, MrpRunLogResource, MrpRunLogResource, MaterialRequirementsSearchResource> MrpRunLogFacadeService { get; private set; }
     
        protected IMaterialRequirementsPlanningFacadeService MaterialRequirementsPlanningFacadeService { get; private set; }

        protected ISingleRecordFacadeResourceService<MrMaster, MrMasterResource> MasterRecordFacadeService { get; private set; }

        protected IMrUsedOnReportService MockUsedOnReportDomainService { get; private set; }

        protected IMrUsedOnReportFacadeService UsedOnReportFacadeService { get; private set; }

        [SetUp]
        public void EstablishContext()
        {
            this.MrpRunLogFacadeService = Substitute
                .For<IFacadeResourceFilterService<MrpRunLog, int, MrpRunLogResource, MrpRunLogResource, MaterialRequirementsSearchResource>>();
            this.MaterialRequirementsPlanningFacadeService = Substitute.For<IMaterialRequirementsPlanningFacadeService>();
            this.MasterRecordFacadeService = Substitute.For<ISingleRecordFacadeResourceService<MrMaster, MrMasterResource>>();
            this.MockUsedOnReportDomainService = Substitute.For<IMrUsedOnReportService>();
            this.UsedOnReportFacadeService = new MrUsedOnReportFacadeService(
                this.MockUsedOnReportDomainService,
                new ResultsModelResourceBuilder());
            this.Client = TestClient.With<MaterialRequirementsModule>(
                services =>
                    {
                        services.AddSingleton(this.MrpRunLogFacadeService);
                        services.AddSingleton(this.MaterialRequirementsPlanningFacadeService);
                        services.AddSingleton(this.MasterRecordFacadeService);
                        services.AddSingleton(this.UsedOnReportFacadeService);
                        services.AddHandlers();
                    },
                FakeAuthMiddleware.EmployeeMiddleware);
        }
    }
}
