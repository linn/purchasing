namespace Linn.Purchasing.Facade.Tests.ResourceBuilderTests.PartSupplierResourceBuilderTests
{
    using Linn.Common.Authorisation;
    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Facade.ResourceBuilders;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IBuilder<PartSupplier> Sut { get; private set; }

        protected IAuthorisationService AuthService { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.AuthService = Substitute.For<IAuthorisationService>();
            this.Sut = new PartSupplierResourceBuilder(this.AuthService);
        }
    }
}
