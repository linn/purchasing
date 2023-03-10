namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeStatusReportServiceTests
{
    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.Reports;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IChangeStatusReportService Sut { get; private set; }

        protected IQueryRepository<ChangeRequest> ChangeRequestsRepository { get; private set; }

        protected IQueryRepository<MrHeader> MrHeaderRepository { get; private set; }

        protected IRepository<Employee, int> EmployeeRepository { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.ChangeRequestsRepository = Substitute.For<IQueryRepository<ChangeRequest>>();
            this.MrHeaderRepository = Substitute.For<IQueryRepository<MrHeader>>();
            this.EmployeeRepository = Substitute.For<IRepository<Employee, int>>();
            this.Sut = new ChangeStatusReportService(
                this.ChangeRequestsRepository,
                this.EmployeeRepository,
                this.MrHeaderRepository,
                new ReportingHelper());
        }
    }
}