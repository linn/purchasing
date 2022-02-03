namespace Linn.Purchasing.Domain.LinnApps.Tests.SupplierServiceTests
{
    using Linn.Common.Authorisation;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected ISupplierService Sut { get; set; }

        protected IAuthorisationService MockAuthorisationService { get; set; }

        [SetUp]
        public void EstablishContext()
        {
            this.MockAuthorisationService = Substitute.For<IAuthorisationService>();
        }
    }
}
