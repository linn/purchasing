namespace Linn.Purchasing.Integration.Tests.BomTypeChangeModuleTests
{
    using System.Net.Http;

    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
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

        protected IQueryRepository<Part> PartRepository { get; private set; }

        protected IAutocostPack AutocostPack { get; private set; }

        protected ICurrencyPack CurrencyPack { get; private set; }

        protected IPartService PartService { get; private set; }

        protected IPartFacadeService FacadeService { get; private set; }

        protected IAuthorisationService AuthorisationService { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.PartRepository = Substitute.For<IQueryRepository<Part>>();
            this.AutocostPack = Substitute.For<IAutocostPack>();
            this.CurrencyPack = Substitute.For<ICurrencyPack>();
            this.PartService = Substitute.For<IPartService>();
            this.AuthorisationService = Substitute.For<IAuthorisationService>();
            this.FacadeService = new PartFacadeService(
                this.PartRepository,
                this.AutocostPack,
                this.CurrencyPack,
                this.PartService,
                new BomTypeChangeResourceBuilder(this.AuthorisationService));

            this.Client = TestClient.With<BomTypeChangeModule>(
                services =>
                    {
                        services.AddSingleton(this.FacadeService);
                        services.AddHandlers();
                    },
                FakeAuthMiddleware.EmployeeMiddleware);
        }
    }
}
