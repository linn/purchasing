namespace Linn.Purchasing.Integration.Tests.ChangeRequestModuleTests
{
    using System.Net.Http;

    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Edi;
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

        [SetUp]
        public void SetUpContext()
        {
            this.Repository = Substitute.For<IRepository<ChangeRequest, int>>();
            this.TransactionManager = Substitute.For<ITransactionManager>();
            this.AuthService = Substitute.For<IAuthorisationService>();
            this.FacadeService = new ChangeRequestFacadeService(
                this.Repository,
                this.TransactionManager,
                new ChangeRequestResourceBuilder(new BomChangeResourceBuilder(), new PcasChangeResourceBuilder(), this.AuthService),
                new ChangeRequestService(this.AuthService, this.Repository));

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
