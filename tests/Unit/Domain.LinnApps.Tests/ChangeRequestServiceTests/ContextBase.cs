namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestServiceTests
{
    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    using NSubstitute;
    using NUnit.Framework;

    public class ContextBase
    {
        protected IChangeRequestService Sut { get; private set; }

        protected IRepository<ChangeRequest, int> Repository { get; private set; }

        protected IAuthorisationService AuthService { get; private set; }

        protected IQueryRepository<Part> PartRepository { get; private set; }

        protected IRepository<Employee, int> EmployeeRepository { get; private set; }

        protected IRepository<LinnWeek, int> WeekRepository { get; private set; }

        protected IRepository<CircuitBoard, string> BoardRepository { get; set; }

        protected IBomPack BomPack { get; private set; }

        protected IPcasPack PcasPack { get; private set; }

        protected IBomChangeService BomChangeService { get; private set; }

        protected IRepository<BomDetail, int> BomDetailRepository { get; private set; }
        
        protected IQueryRepository<PartUsedOn> PartUsedOnRepository { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.Repository = Substitute.For<IRepository<ChangeRequest, int>>();
            this.AuthService = Substitute.For<IAuthorisationService>();
            this.PartRepository = Substitute.For<IQueryRepository<Part>>();
            this.EmployeeRepository = Substitute.For<IRepository<Employee, int>>();
            this.WeekRepository = Substitute.For<IRepository<LinnWeek, int>>();
            this.BoardRepository = Substitute.For<IRepository<CircuitBoard, string>>();
            this.BomDetailRepository = Substitute.For<IRepository<BomDetail, int>>();
            this.PartUsedOnRepository = Substitute.For<IQueryRepository<PartUsedOn>>();
            this.BomPack = Substitute.For<IBomPack>();
            this.PcasPack = Substitute.For<IPcasPack>();
            this.BomChangeService = Substitute.For<IBomChangeService>();
            this.Sut = new ChangeRequestService(
                this.AuthService,
                this.Repository,
                this.PartRepository,
                this.EmployeeRepository,
                this.WeekRepository,
                this.BomPack,
                this.PcasPack,
                this.BomChangeService,
                this.BoardRepository,
                this.BomDetailRepository,
                this.PartUsedOnRepository);
        }
    }
}
