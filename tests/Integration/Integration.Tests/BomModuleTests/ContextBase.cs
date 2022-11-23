namespace Linn.Purchasing.Integration.Tests.BomModuleTests
{
    using System.Net.Http;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Edi;
    using Linn.Purchasing.Facade.ResourceBuilders;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.IoC;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.Boms;
    using Linn.Purchasing.Service.Modules;

    using Microsoft.Extensions.DependencyInjection;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected HttpResponseMessage Response { get; set; }

        protected IFacadeResourceService<Bom, int, BomResource, BomResource> FacadeService { get; set; }

        protected IFacadeResourceService<CircuitBoard, string, CircuitBoardResource, CircuitBoardResource> CircuitBoardFacadeService { get; set; }

        protected IRepository<Bom, int> Repository { get; set; }

        protected IRepository<CircuitBoard, string> CircuitBoardRepository { get; set; }

        protected IEdiOrderService MockDomainService { get; set; }

        protected ITransactionManager TransactionManager { get; set; }

        protected IBomTreeService BomTreeService { get; private set; }

        protected IBomTreeReportsService BomTreeReportsService { get; private set; }

        protected IBomReportsFacadeService BomReportsFacadeService { get; private set; }

        protected IBomReportsService MockBomReportsDomainService { get; set; }

        [SetUp]
        public void SetUpContext()
        {
            this.Repository = Substitute.For<IRepository<Bom, int>>();
            this.CircuitBoardRepository = Substitute.For<IRepository<CircuitBoard, string>>();
            this.TransactionManager = Substitute.For<ITransactionManager>();
            this.FacadeService = new BomFacadeService(
                this.Repository,
                this.TransactionManager,
                new BomResourceBuilder());

            this.BomTreeService = Substitute.For<IBomTreeService>();

            this.MockBomReportsDomainService = Substitute.For<IBomReportsService>();

            this.BomTreeReportsService = new BomTreeReportService(this.BomTreeService);

            this.BomReportsFacadeService = new BomReportsFacadeService(
                this.MockBomReportsDomainService,
                new ReportReturnResourceBuilder());

            this.CircuitBoardFacadeService = new CircuitBoardFacadeService(
                this.CircuitBoardRepository,
                this.TransactionManager,
                new CircuitBoardResourceBuilder());

            this.Client = TestClient.With<BomModule>(
                services =>
                    {
                        services.AddSingleton(this.FacadeService);
                        services.AddSingleton(this.BomTreeReportsService);
                        services.AddSingleton(this.CircuitBoardFacadeService);
                        services.AddSingleton(this.BomReportsFacadeService);
                        services.AddHandlers();
                    },
                FakeAuthMiddleware.EmployeeMiddleware);
        }
    }
}
