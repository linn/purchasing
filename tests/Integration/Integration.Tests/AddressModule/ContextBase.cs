namespace Linn.Purchasing.Integration.Tests.AddressModule
{
    using System.Net.Http;

    using Linn.Common.Facade;
    using Linn.Common.Logging;
    using Linn.Common.Persistence;
    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Facade.ResourceBuilders;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.IoC;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Service.Modules;

    using Microsoft.Extensions.DependencyInjection;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected HttpResponseMessage Response { get; set; }

        protected IDatabaseService DatabaseService { get; private set; }

        protected ITransactionManager TransactionManager { get; set; }

        protected IFacadeResourceFilterService<Address, int, AddressResource, AddressResource, AddressResource>
            FacadeService
        {
            get; private set;
        }

        protected ILog Log { get; private set; }

        protected IRepository<Address, int> AddressRepository { get; private set; }

        protected IRepository<FullAddress, int> FullAddressRepository { get; private set; }


        protected IRepository<Country, string> CountryRepository { get; private set; }

        protected IFacadeResourceService<Country, string, CountryResource, CountryResource> CountryService { get; private set; }

        [SetUp]
        public void EstablishContext()
        {
            this.TransactionManager = Substitute.For<ITransactionManager>();
            this.AddressRepository = Substitute.For<IRepository<Address, int>>();
            this.FullAddressRepository = Substitute.For<IRepository<FullAddress, int>>();
            this.CountryRepository = Substitute.For<IRepository<Country, string>>();
            this.Log = Substitute.For<ILog>();
            this.DatabaseService = Substitute.For<IDatabaseService>();

            this.FacadeService = new AddressService(
                this.AddressRepository,
                this.TransactionManager,
                new AddressResourceBuilder(this.FullAddressRepository),
                this.CountryRepository,
                this.DatabaseService);

            this.CountryService = new CountryService(
                this.CountryRepository,
                this.TransactionManager,
                new CountryResourceBuilder());

            this.Client = TestClient.With<AddressModule>(
                services =>
                    {
                        services.AddSingleton(this.TransactionManager);
                        services.AddSingleton(this.Log);

                        services.AddSingleton(this.FacadeService);

                        services.AddSingleton(this.CountryService);

                        services.AddHandlers();
                    },
                FakeAuthMiddleware.EmployeeMiddleware);
        }
    }
}
