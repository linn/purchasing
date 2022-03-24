namespace Linn.Purchasing.Integration.Tests.MaterialRequirementsModuleTests
{
    using System.Net.Http;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.IoC;
    using Linn.Purchasing.Resources.MaterialRequirements;
    using Linn.Purchasing.Service.Modules;

    using Microsoft.Extensions.DependencyInjection;

    using NSubstitute;

    using NUnit.Framework;

    public abstract class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected HttpResponseMessage Response { get; set; }

        protected IFacadeResourceService<MrpRunLog, int, MrpRunLogResource, MrpRunLogResource> MrpRunLogFacadeService { get; private set; }

        [SetUp]
        public void EstablishContext()
        {
            this.MrpRunLogFacadeService = Substitute.For<IFacadeResourceService<MrpRunLog, int, MrpRunLogResource, MrpRunLogResource>>();
            this.Client = TestClient.With<MaterialRequirementsModule>(
                services =>
                    {
                        services.AddSingleton(this.MrpRunLogFacadeService);
                        services.AddHandlers();
                    },
                FakeAuthMiddleware.EmployeeMiddleware);
        }
    }
}
