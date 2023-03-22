namespace Linn.Purchasing.Domain.LinnApps.Tests.BomStandardPriceServiceTests
{
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IBomStandardPriceService Sut { get; private set; }

        protected IQueryRepository<BomStandardPrice> Repository { get; private set; }

        protected IAutocostPack AutocostPack { get; private set; }

        protected IStoresMatVarPack StoresMatVarPack { get; private set;  }

        [SetUp]
        public void Setup()
        {
            this.StoresMatVarPack = Substitute.For<IStoresMatVarPack>();
            this.Repository = Substitute.For<IQueryRepository<BomStandardPrice>>();
            this.AutocostPack = Substitute.For<IAutocostPack>();
            this.Sut = new BomStandardPriceService(
                this.Repository, this.StoresMatVarPack, this.AutocostPack);
        }
    }
}
