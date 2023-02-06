namespace Linn.Purchasing.Integration.Tests.ChangeRequestModuleTests
{
    using System.Net.Http;

    using Linn.Common.Authorisation;
    using Linn.Common.Logging;
    using Linn.Common.Persistence;
    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Edi;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Facade.ResourceBuilders;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.IoC;
    using Linn.Purchasing.Service.Modules;

    using Microsoft.Extensions.DependencyInjection;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected HttpResponseMessage Response { get; set; }

        protected IChangeRequestFacadeService FacadeService { get; set; }

        protected IRepository<ChangeRequest, int> Repository { get; set; }

        protected IEdiOrderService MockDomainService { get; set; }

        protected ITransactionManager TransactionManager { get; set; }

        protected IAuthorisationService AuthService { get; set; }

        protected IDatabaseService DatabaseService { get; set; }

        protected IQueryRepository<Part> PartRepository { get; set; }

        protected IRepository<Employee, int> EmployeeRepository { get; set; }

        protected IRepository<LinnWeek, int> WeekRepository { get; set; }

        protected IBomTreeService BomTreeService { get; private set; }

        protected IBomPack BomPack { get; private set; }

        protected IPcasPack PcasPack { get; private set; }

        protected ILog Logger { get; set; }

        [SetUp]
        public void SetUpContext()
        {
            this.Repository = Substitute.For<IRepository<ChangeRequest, int>>();
            this.TransactionManager = Substitute.For<ITransactionManager>();
            this.AuthService = Substitute.For<IAuthorisationService>();
            this.DatabaseService = Substitute.For<IDatabaseService>();
            this.PartRepository = Substitute.For<IQueryRepository<Part>>();
            this.EmployeeRepository = Substitute.For<IRepository<Employee, int>>();
            this.WeekRepository = Substitute.For<IRepository<LinnWeek, int>>();
            this.Logger = Substitute.For<ILog>();
            this.BomTreeService = Substitute.For<IBomTreeService>();
            this.BomPack = Substitute.For<IBomPack>();
            this.PcasPack = Substitute.For<IPcasPack>();

            this.FacadeService = new ChangeRequestFacadeService(
                this.Repository,
                this.TransactionManager,
                new ChangeRequestResourceBuilder(
                    new BomChangeResourceBuilder(), new PcasChangeResourceBuilder(), this.AuthService),
                new ChangeRequestService(
                    this.AuthService,
                    this.Repository,
                    this.PartRepository,
                    this.EmployeeRepository,
                    this.WeekRepository,
                    this.BomPack,
                    this.PcasPack),
                    this.DatabaseService,
                    this.BomTreeService,
                    this.Logger);

            this.Client = TestClient.With<ChangeRequestModule>(
                services =>
                    {
                        services.AddSingleton(this.FacadeService);
                        services.AddHandlers();
                    },
                FakeAuthMiddleware.EmployeeMiddleware);
        }
    }
}
