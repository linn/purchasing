namespace Linn.Purchasing.Domain.LinnApps.Tests.BomVerificationHistoryServiceTests
{
    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    using NSubstitute;
    using NUnit.Framework;
    using System.Net.Http;

    public class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected HttpResponseMessage Response { get; set; }

        protected IBomVerificationHistoryService Sut { get; private set; }

        protected IAuthorisationService AuthService { get; private set; }

        protected IDatabaseService DatabaseService { get; private set; }

        protected IRepository<Employee, int> EmployeeRepository { get; private set; }

        protected IRepository<BomVerificationHistory, int> BomVerificationHistoryRepository { get; private set; }

        protected IQueryRepository<Part> PartRepository { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.BomVerificationHistoryRepository = Substitute.For<IRepository<BomVerificationHistory, int>>();
            this.AuthService = Substitute.For<IAuthorisationService>();
            this.PartRepository = Substitute.For<IQueryRepository<Part>>();
            this.DatabaseService = Substitute.For<IDatabaseService>();
            this.EmployeeRepository = Substitute.For<IRepository<Employee, int>>();
            this.Sut = new BomVerificationHistoryService(
                this.AuthService,
                this.DatabaseService,
                this.PartRepository,
                this.BomVerificationHistoryRepository,
                this.EmployeeRepository);
        }
    }
}
