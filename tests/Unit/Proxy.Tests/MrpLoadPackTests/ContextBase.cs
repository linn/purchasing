namespace Linn.Purchasing.Proxy.Tests.MrpLoadPackTests
{
    using Linn.Common.Proxy.LinnApps;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected MrpLoadPack Sut { get; private set; }

        protected IDatabaseService DatabaseService { get; private set;  }

        [SetUp]
        public void SetUpContext()
        {
            this.DatabaseService = Substitute.For<IDatabaseService>();

            this.Sut = new MrpLoadPack(this.DatabaseService);
        }
    }
}
