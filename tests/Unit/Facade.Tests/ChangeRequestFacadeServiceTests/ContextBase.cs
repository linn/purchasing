namespace Linn.Purchasing.Facade.Tests.ChangeRequestFacadeServiceTests
{
    using Linn.Common.Authorisation;
    using Linn.Common.Logging;
    using Linn.Common.Persistence;
    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Facade.ResourceBuilders;
    using Linn.Purchasing.Facade.Services;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IRepository<ChangeRequest, int> Repository { get; private set; }

        protected ITransactionManager TransactionManager { get; private set; }

        protected IAuthorisationService AuthorisationService { get; private set; }

        protected IDatabaseService DatabaseService { get; private set; }

        protected IQueryRepository<Part> PartRepository { get; private set; }

        protected IRepository<Employee, int> EmployeeRepository { get; private set; }

        protected IRepository<LinnWeek, int> WeekRepository { get; private set; }

        protected IRepository<CircuitBoard, string> BoardRepository { get; private set; }

        protected IBomPack BomPack { get; private set; }

        protected IPcasPack PcasPack { get; private set; }

        protected IBomChangeService BomChangeService { get; private set; }

        protected ILog Logger { get; set; }

        protected IBomTreeService BomTreeService { get; private set; }

        protected ChangeRequestFacadeService Sut { get; private set; }

        protected IRepository<BomDetail, int> BomDetailRepository { get; private set; }
        
        protected IQueryRepository<PartUsedOn> PartUsedOnRepository { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.Repository = Substitute.For<IRepository<ChangeRequest, int>>();
            this.TransactionManager = Substitute.For<ITransactionManager>();
            this.AuthorisationService = Substitute.For<IAuthorisationService>();
            this.DatabaseService = Substitute.For<IDatabaseService>();
            this.PartRepository = Substitute.For<IQueryRepository<Part>>();
            this.EmployeeRepository = Substitute.For<IRepository<Employee, int>>();
            this.WeekRepository = Substitute.For<IRepository<LinnWeek, int>>();
            this.BoardRepository = Substitute.For<IRepository<CircuitBoard, string>>();
            this.BomTreeService = Substitute.For<IBomTreeService>();
            this.BomPack = Substitute.For<IBomPack>();
            this.PcasPack = Substitute.For<IPcasPack>();
            this.BomChangeService = Substitute.For<IBomChangeService>();
            this.BomDetailRepository = Substitute.For<IRepository<BomDetail, int>>();
            this.PartUsedOnRepository = Substitute.For<IQueryRepository<PartUsedOn>>();
            this.Logger = Substitute.For<ILog>();

            this.Sut = new ChangeRequestFacadeService(
                this.Repository,
                this.TransactionManager,
                new ChangeRequestResourceBuilder(
                new BomChangeResourceBuilder(),
                new PcasChangeResourceBuilder(),
                this.AuthorisationService),
                new ChangeRequestService(
                this.AuthorisationService,
                this.Repository,
                this.PartRepository,
                this.EmployeeRepository,
                this.WeekRepository,
                this.BomPack,
                this.PcasPack,
                this.BomChangeService,
                this.BoardRepository,
                this.BomDetailRepository,
                this.PartUsedOnRepository),
                this.DatabaseService, 
                this.BomTreeService,
                this.Logger);
        }
    }
}
