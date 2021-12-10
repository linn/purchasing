namespace Linn.Purchasing.Domain.LinnApps.Tests.PartSupplierServiceTests
{
    using Linn.Common.Authorisation;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IPartSupplierService Sut { get; private set; }

        protected IAuthorisationService MockAuthService { get; set; }

        [SetUp]
        public void SetUpContext()
        {
            this.MockAuthService = Substitute.For<IAuthorisationService>();
            this.Sut = new PartSupplierService(this.MockAuthService);
        }
    }
}
