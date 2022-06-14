namespace Linn.Purchasing.Facade.Tests.ResourceBuilderTests.MrReportResourceBuilderTests
{
    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Facade.ResourceBuilders;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IBuilder<MrReport> Sut { get; private set; }

        protected string JobRef { get; private set; }

        protected MrReport Report { get; set; }
        
        protected MrHeader MrHeader1 { get; set; }
        
        protected MrHeader MrHeader2 { get; set; }


        [SetUp]
        public void SetUpContext()
        {
            this.JobRef = "abc";

            this.Sut = new MrReportResourceBuilder();
        }
    }
}
