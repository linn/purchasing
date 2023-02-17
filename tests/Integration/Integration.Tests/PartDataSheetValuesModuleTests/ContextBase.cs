namespace Linn.Purchasing.Integration.Tests.PartDataSheetValuesModuleTests
{
    using System.Net.Http;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Facade.ResourceBuilders;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Integration.Tests;
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

        protected IFacadeResourceService<PartDataSheetValues, PartDataSheetValuesKey, PartDataSheetValuesResource, PartDataSheetValuesResource> Service { get; private set; }

        protected IRepository<PartDataSheetValues, PartDataSheetValuesKey> PartDataSheetValuesRepository { get; private set; }

        protected ITransactionManager TransactionManager { get; private set; }

        [SetUp]
        public void EstablishContext()
        {
            this.PartDataSheetValuesRepository = Substitute.For<IRepository<PartDataSheetValues, PartDataSheetValuesKey>>();
            this.TransactionManager = Substitute.For<ITransactionManager>();

            this.Service = new PartDataSheetValuesService(
                this.PartDataSheetValuesRepository,
                this.TransactionManager,
                new PartDataSheetValuesResourceBuilder());
            this.Client = TestClient.With<PartDataSheetValuesModule>(
                services =>
                    {
                        services.AddSingleton(this.Service);
                        services.AddHandlers();
                    },
                FakeAuthMiddleware.EmployeeMiddleware);
        }
    }
}
