namespace Linn.Purchasing.Domain.LinnApps.Tests.PartServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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

        protected IQueryRepository<Part> partRepository;

        protected IRepository<PartSupplier, PartSupplierKey> partSupplierRepository;

        protected IPartHistoryService partHistoryService;

        protected IAuthorisationService authService;

        [SetUp]
        public void SetUpContext()
        {
            this.partRepository = Substitute.For<IQueryRepository<Part>>();
            this.partSupplierRepository = Substitute.For<IRepository<PartSupplier, PartSupplierKey>>();
            this.partHistoryService = Substitute.For<IPartHistoryService>();
            this.authService = Substitute.For<IAuthorisationService>();
            this.Sut = new PartService(this.partRepository, this.partSupplierRepository, this.partHistoryService, this.authService);
        }
    }
}
