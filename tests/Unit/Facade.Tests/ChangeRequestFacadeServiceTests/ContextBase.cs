namespace Linn.Purchasing.Facade.Tests.ChangeRequestFacadeServiceTests
{
    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Boms;
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

        protected ChangeRequestFacadeService Sut { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.Repository = Substitute.For<IRepository<ChangeRequest, int>>();
            this.TransactionManager = Substitute.For<ITransactionManager>();
            this.AuthorisationService = Substitute.For<IAuthorisationService>();
            this.DatabaseService = Substitute.For<IDatabaseService>();
            this.PartRepository = Substitute.For<IQueryRepository<Part>>();
            this.EmployeeRepository = Substitute.For<IRepository<Employee, int>>();

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
                    this.EmployeeRepository),
                this.DatabaseService);
        }
    }
}
