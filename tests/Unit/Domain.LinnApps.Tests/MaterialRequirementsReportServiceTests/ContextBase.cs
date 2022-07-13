namespace Linn.Purchasing.Domain.LinnApps.Tests.MaterialRequirementsReportServiceTests
{
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IQueryRepository<MrHeader> MrHeaderRepository { get; private set; }

        protected ISingleRecordRepository<MrMaster> MrMasterRecordRepository { get; private set; }

        protected IRepository<MrpRunLog, int> RunLogRepository { get; private set; }
        
        protected IRepository<Planner, int> PlannerRepository { get; private set; }

        protected IRepository<Employee, int> EmployeeRepository { get; private set; }

        protected MaterialRequirementsReportService Sut { get; private set; }

        protected IQueryRepository<MrPurchaseOrderDetail> MrPurchaseOrderDetailRepository { get; private set; }
        
        protected IQueryRepository<PartAndAssembly> PartsAndAssembliesRepository { get; private set; }
        
        protected IRepository<PartNumberList, string> PartNumberListRepository { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.MrHeaderRepository = Substitute.For<IQueryRepository<MrHeader>>();
            this.RunLogRepository = Substitute.For<IRepository<MrpRunLog, int>>();
            this.PlannerRepository = Substitute.For<IRepository<Planner, int>>();
            this.EmployeeRepository = Substitute.For<IRepository<Employee, int>>();
            this.MrMasterRecordRepository = Substitute.For<ISingleRecordRepository<MrMaster>>();
            this.MrPurchaseOrderDetailRepository = Substitute.For<IQueryRepository<MrPurchaseOrderDetail>>();
            this.PartsAndAssembliesRepository = Substitute.For<IQueryRepository<PartAndAssembly>>();
            this.PartNumberListRepository = Substitute.For<IRepository<PartNumberList, string>>();
            this.Sut = new MaterialRequirementsReportService(
                this.MrHeaderRepository,
                this.RunLogRepository,
                this.MrMasterRecordRepository,
                this.PlannerRepository,
                this.EmployeeRepository,
                this.MrPurchaseOrderDetailRepository,
                this.PartsAndAssembliesRepository,
                this.PartNumberListRepository);
        }
    }
}
