namespace Linn.Purchasing.Domain.LinnApps.Tests.WhatsInInspectionReportServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenDefaultParameters : ContextBase
    {
        private WhatsInInspectionReport result;

        [SetUp]
        public void SetUp()
        {
            this.result = this.Sut.GetReport();
        }
    }
}
