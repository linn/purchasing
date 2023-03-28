namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeStatusReportServiceTests
{
    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Domain.LinnApps.Reports;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IChangeStatusReportService Sut { get; private set; }

        protected IQueryRepository<ChangeRequest> ChangeRequestsRepository { get; private set; }

        protected IQueryRepository<ChangeRequestPhaseInWeeksView> ChangeRequestPhaseInWeeksView { get; private set; }

        protected IRepository<LinnWeek, int> WeekRepository { get; private set; }

        protected IRepository<Employee, int> EmployeeRepository { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.ChangeRequestsRepository = Substitute.For<IQueryRepository<ChangeRequest>>();
            this.WeekRepository = Substitute.For<IRepository<LinnWeek, int>>();
            this.EmployeeRepository = Substitute.For<IRepository<Employee, int>>();
            this.ChangeRequestPhaseInWeeksView = Substitute.For<IQueryRepository<ChangeRequestPhaseInWeeksView>>();
            this.Sut = new ChangeStatusReportService(
                this.ChangeRequestsRepository,
                this.WeekRepository,
                this.EmployeeRepository,
                this.ChangeRequestPhaseInWeeksView,
                new ReportingHelper());
        }
    }
}