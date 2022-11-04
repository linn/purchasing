namespace Linn.Purchasing.Domain.LinnApps.Tests.PartServiceTests
{
    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IPartService Sut { get; private set; }

        protected IQueryRepository<Part> PartRepository { get; private set; }

        protected IRepository<PartSupplier, PartSupplierKey> PartSupplierRepository { get; private set; }

        protected IPartHistoryService PartHistoryService { get; private set; }

        protected IAuthorisationService AuthService { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.PartRepository = Substitute.For<IQueryRepository<Part>>();
            this.PartSupplierRepository = Substitute.For<IRepository<PartSupplier, PartSupplierKey>>();
            this.PartHistoryService = Substitute.For<IPartHistoryService>();
            this.AuthService = Substitute.For<IAuthorisationService>();
            this.Sut = new PartService(
                this.PartRepository,
                this.PartSupplierRepository,
                this.PartHistoryService,
                this.AuthService);
        }
    }
}
