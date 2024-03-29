﻿namespace Linn.Purchasing.Facade.Tests.ResourceBuilderTests.MrPurchaseOrderResourceBuilderTests
{
    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Facade.ResourceBuilders;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IBuilder<MrPurchaseOrderDetail> Sut { get; private set; }

        protected string JobRef { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.JobRef = "abc";

            this.Sut = new MrPurchaseOrderResourceBuilder();
        }
    }
}
