namespace Linn.Purchasing.Facade.Tests.PartFacadeServiceTests
{
    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Facade.ResourceBuilders;
    using Linn.Purchasing.Facade.Services;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IQueryRepository<Part> partRepository;

        protected IPartService partService; 

        protected IAutocostPack autocostPack;

        protected ICurrencyPack currencyPack;

        protected IAuthorisationService authorisationService;

        protected PartFacadeService Sut { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.partRepository = Substitute.For<IQueryRepository<Part>>();
            this.autocostPack = Substitute.For<IAutocostPack>();
            this.currencyPack = Substitute.For<ICurrencyPack>();
            this.partService = Substitute.For<IPartService>();
            this.authorisationService = Substitute.For<IAuthorisationService>();

            this.Sut = new PartFacadeService(this.partRepository, this.autocostPack, this.currencyPack, this.partService, new 
                BomTypeChangeResourceBuilder(this.authorisationService));
        }
    }
}
