namespace Linn.Purchasing.Domain.LinnApps.Tests.BomChangeServiceTests
{
    using Linn.Common.Persistence;
    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IBomChangeService Sut { get; private set; }

        protected IRepository<BomChange, int> BomChangeRepository { get; private set; }

        protected IRepository<BomDetail, int> BomDetailRepository { get; private set; }

        protected IRepository<Bom, int> BomRepository { get; private set; }

        protected IDatabaseService DatabaseService { get; private set; }

        protected IQueryRepository<Part> PartRepository { get; private set; }

        protected IBomPack BomPack { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.BomChangeRepository = Substitute.For<IRepository<BomChange, int>>();
            this.BomDetailRepository = Substitute.For<IRepository<BomDetail, int>>();
            this.BomRepository = Substitute.For<IRepository<Bom, int>>();
            this.PartRepository = Substitute.For<IQueryRepository<Part>>();
            this.DatabaseService = Substitute.For<IDatabaseService>();
            this.BomPack = Substitute.For<IBomPack>();
            this.Sut = new BomChangeService(
                this.DatabaseService,
                this.BomChangeRepository,
                this.BomDetailRepository,
                this.BomRepository,
                this.PartRepository,
                this.BomPack);
        }
    }
}
