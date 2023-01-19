namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestServiceTests
{
    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Boms;
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

        [SetUp]
        public void SetUpContext()
        {
            this.Repository = Substitute.For<IRepository<ChangeRequest, int>>();
            this.AuthService = Substitute.For<IAuthorisationService>();
            this.PartRepository = Substitute.For<IQueryRepository<Part>>();
            this.EmployeeRepository = Substitute.For<IRepository<Employee, int>>();
            this.WeekRepository = Substitute.For<IRepository<LinnWeek, int>>();
            this.Sut = new ChangeRequestService(
                this.AuthService,
                this.Repository,
                this.PartRepository,
                this.EmployeeRepository,
                this.WeekRepository);
        }
    }
}
