namespace Linn.Purchasing.Facade.Tests.MaterialRequirementsPlanningFacadeServiceTests
{
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Facade.Services;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        public MaterialRequirementsPlanningFacadeService Sut { get; set; }

        public IMaterialRequirementsPlanningService MaterialRequirementsPlanningService { get; set; }

        [SetUp]
        public void SetUpContext()
        {
            this.MaterialRequirementsPlanningService = Substitute.For<IMaterialRequirementsPlanningService>();
            this.Sut = new MaterialRequirementsPlanningFacadeService(this.MaterialRequirementsPlanningService);
        }
    }
}
