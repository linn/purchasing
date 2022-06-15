namespace Linn.Purchasing.Facade.Tests.MaterialRequirementsReportFacadeServiceTests
{
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Facade.ResourceBuilders;
    using Linn.Purchasing.Facade.Services;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        public MaterialRequirementsReportFacadeService Sut { get; set; }

        public IMaterialRequirementsReportService MaterialRequirementsReportService { get; set; }

        [SetUp]
        public void SetUpContext()
        {
            this.MaterialRequirementsReportService = Substitute.For<IMaterialRequirementsReportService>();
            this.Sut = new MaterialRequirementsReportFacadeService(
                this.MaterialRequirementsReportService,
                new MrReportResourceBuilder(),
                new MrReportOptionsResourceBuilder(),
                new MrPurchaseOrderResourceBuilder());
        }
    }
}
