namespace Linn.Purchasing.Domain.LinnApps.Tests.SupplierServiceTests
{
    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected ISupplierService Sut { get; set; }

        protected IAuthorisationService MockAuthorisationService { get; set; }

        protected IRepository<Supplier, int> MockSupplierRepository { get; set; }

        protected IRepository<Currency, string> MockCurrencyRepository { get; set; }

        [SetUp]
        public void EstablishContext()
        {
            this.MockAuthorisationService = Substitute.For<IAuthorisationService>();
            this.MockSupplierRepository = Substitute.For<IRepository<Supplier, int>>();
            this.MockCurrencyRepository = Substitute.For<IRepository<Currency, string>>();
        }
    }
}
