namespace Linn.Purchasing.Domain.LinnApps.Tests.MaterialRequirementsPlanningServiceTests
{
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected MaterialRequirementsPlanningService Sut { get; private set; }

        protected IMrpLoadPack MrpLoadPack { get; private set;  }

        [SetUp]
        public void SetUpContext()
        {
            this.MrpLoadPack = Substitute.For<IMrpLoadPack>();

            this.Sut = new MaterialRequirementsPlanningService(this.MrpLoadPack);
        }
    }
}
