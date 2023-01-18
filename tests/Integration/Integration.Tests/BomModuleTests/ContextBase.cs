namespace Linn.Purchasing.Integration.Tests.BomModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Domain.LinnApps.Edi;
    using Linn.Purchasing.Facade.ResourceBuilders;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.IoC;
    using Linn.Purchasing.Resources.Boms;
    using Linn.Purchasing.Service.Modules;

    using Microsoft.Extensions.DependencyInjection;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected HttpResponseMessage Response { get; set; }

        protected IBomFacadeService FacadeService { get; set; }

        protected IFacadeResourceService<CircuitBoard, string, CircuitBoardResource, CircuitBoardResource> CircuitBoardFacadeService { get; set; }

        protected IRepository<Bom, int> Repository { get; set; }

        protected IRepository<CircuitBoard, string> CircuitBoardRepository { get; set; }

        protected IRepository<BoardRevisionType, string> BoardRevisionTypeRepository { get; set; }
        
        protected IQueryRepository<BoardComponentSummary> BoardComponentSummaryRepository { get; set; }

        protected IRepository<PcasChange, int> PcasChangeRepository { get; set; }

        protected IEdiOrderService MockDomainService { get; set; }

        protected ITransactionManager TransactionManager { get; set; }

        protected IBomTreeService BomTreeService { get; private set; }

        protected IBomTreeReportsService BomTreeReportsService { get; private set; }

        protected IBomReportsFacadeService BomReportsFacadeService { get; private set; }

        protected IQueryFacadeResourceService<BoardComponentSummary, BoardComponentSummaryResource, BoardComponentSummaryResource> BoardComponentSummaryFacadeService { get; private set; }

        protected IBomReportsService MockBomReportsDomainService { get; set; }

        protected IBomChangeService BomChangeService { get; set; }
        
        protected string BoardCode { get; set; }

        protected CircuitBoardResource Resource { get; set; }


        [SetUp]
        public void SetUpContext()
        {
            this.Repository = Substitute.For<IRepository<Bom, int>>();
            this.CircuitBoardRepository = Substitute.For<IRepository<CircuitBoard, string>>();
            this.BoardRevisionTypeRepository = Substitute.For<IRepository<BoardRevisionType, string>>();
            this.BoardComponentSummaryRepository = Substitute.For<IQueryRepository<BoardComponentSummary>>();
            this.PcasChangeRepository = Substitute.For<IRepository<PcasChange, int>>();
            this.BomChangeService = Substitute.For<IBomChangeService>();
            this.TransactionManager = Substitute.For<ITransactionManager>();
            this.BomTreeService = Substitute.For<IBomTreeService>();

            this.FacadeService = new BomFacadeService(
                this.BomChangeService, this.TransactionManager, this.BomTreeService);

            this.MockBomReportsDomainService = Substitute.For<IBomReportsService>();

            this.BomTreeReportsService = new BomTreeReportService(this.BomTreeService);

            this.BomReportsFacadeService = new BomReportsFacadeService(
                this.MockBomReportsDomainService,
                new ReportReturnResourceBuilder());
            this.BoardComponentSummaryFacadeService = new BoardComponentSummaryFacadeService(
                this.BoardComponentSummaryRepository,
                new BoardComponentSummaryResourceBuilder());
            this.BoardRevisionTypeRepository.FindAll().Returns(
                new List<BoardRevisionType>
                    {
                        new BoardRevisionType { TypeCode = "PRODUCTION" },
                        new BoardRevisionType { TypeCode = "PROTOTYPE" }
                    }.AsQueryable());

            this.CircuitBoardFacadeService = new CircuitBoardFacadeService(
                this.CircuitBoardRepository,
                this.TransactionManager,
                new CircuitBoardResourceBuilder(this.PcasChangeRepository),
                this.BoardRevisionTypeRepository);

            this.BomChangeService = Substitute.For<IBomChangeService>();
              
            this.BoardCode = "808";
            this.Resource = new CircuitBoardResource
            {
                BoardCode = this.BoardCode,
                Description = "Desc",
                ClusterBoard = "Y",
                CoreBoard = "Y",
                IdBoard = "Y",
                SplitBom = "Y",
                Layouts = new List<BoardLayoutResource>
                                                  {
                                                      new BoardLayoutResource
                                                          {
                                                              BoardCode = this.BoardCode,
                                                              LayoutCode = "L1",
                                                              LayoutSequence = 1,
                                                              PcbNumber = "PCB",
                                                              LayoutType = "L",
                                                              LayoutNumber = 1,
                                                              PcbPartNumber = "PCB PART",
                                                              ChangeId = null,
                                                              ChangeState = null,
                                                              Revisions = new List<BoardRevisionResource>
                                                                              {
                                                                                  new BoardRevisionResource
                                                                                      {
                                                                                          BoardCode = this.BoardCode,
                                                                                          LayoutCode = "L1",
                                                                                          RevisionCode = "L1R1",
                                                                                          LayoutSequence = 1,
                                                                                          VersionNumber = 1,
                                                                                          RevisionType =
                                                                                              new
                                                                                              BoardRevisionTypeResource
                                                                                                  {
                                                                                                      TypeCode =
                                                                                                          "PRODUCTION"
                                                                                                  },
                                                                                          RevisionNumber = 1,
                                                                                          SplitBom = "N",
                                                                                          PcasPartNumber = "PCAS",
                                                                                          PcsmPartNumber = "PCSM",
                                                                                          PcbPartNumber = "PCB",
                                                                                          AteTestCommissioned = null,
                                                                                          ChangeId = null,
                                                                                          ChangeState = null
                                                                                      }
                                                                              }
                                                          },
                                                      new BoardLayoutResource
                                                          {
                                                              BoardCode = this.BoardCode,
                                                              LayoutCode = "L2",
                                                              LayoutSequence = 2,
                                                              PcbNumber = "PCB2",
                                                              LayoutType = "L",
                                                              LayoutNumber = 2,
                                                              PcbPartNumber = "PCB PART2",
                                                              ChangeId = null,
                                                              ChangeState = null,
                                                              Revisions = new List<BoardRevisionResource>
                                                                              {
                                                                                  new BoardRevisionResource
                                                                                      {
                                                                                          BoardCode = this.BoardCode,
                                                                                          LayoutCode = "L2",
                                                                                          RevisionCode = "L2R1",
                                                                                          LayoutSequence = 2,
                                                                                          VersionNumber = 1,
                                                                                          RevisionType =
                                                                                              new
                                                                                              BoardRevisionTypeResource
                                                                                                  {
                                                                                                      TypeCode =
                                                                                                          "PRODUCTION"
                                                                                                  },
                                                                                          RevisionNumber = 1,
                                                                                          SplitBom = "N",
                                                                                          PcasPartNumber = "PCAS2",
                                                                                          PcsmPartNumber = "PCSM2",
                                                                                          PcbPartNumber = "PCB2",
                                                                                          AteTestCommissioned = null,
                                                                                          ChangeId = null,
                                                                                          ChangeState = null
                                                                                      }
                                                                              }
                                                          }
                                                  }
            };
            this.Client = TestClient.With<BomModule>(
                services =>
                    {
                        services.AddSingleton(this.FacadeService);
                        services.AddSingleton(this.BomTreeReportsService);
                        services.AddSingleton(this.CircuitBoardFacadeService);
                        services.AddSingleton(this.BomReportsFacadeService);
                        services.AddSingleton(this.BoardComponentSummaryFacadeService);
                        services.AddHandlers();
                    },
                FakeAuthMiddleware.EmployeeMiddleware);
        }
    }
}
