namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderServiceTests
{
    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IPurchaseOrderService Sut { get; private set; }

        protected IAuthorisationService MockAuthService { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.MockAuthService = Substitute.For<IAuthorisationService>();

            this.Sut = new PurchaseOrderService(
                this.MockAuthService);
        }
    }
}
