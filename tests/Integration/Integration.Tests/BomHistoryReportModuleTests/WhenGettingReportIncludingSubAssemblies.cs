namespace Linn.Purchasing.Integration.Tests.BomHistoryReportModuleTests
{
    using System.Collections.Generic;

    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps.Reports.Models;
    using Linn.Purchasing.Integration.Tests.Extensions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingReportIncludingSubAssemblies : ContextBase
    {
        private IEnumerable<BomHistoryReportLine> result;

        [SetUp]
        public void SetUp()
        {
            this.result = new List<BomHistoryReportLine>
                              {
                                  new BomHistoryReportLine()
                                      {
                                          BomName = "SK HUB"
                                      }
                              };
            this.MockDomainService.GetReport("SK HUB", 19.September(2023), 19.September(2023).AddDays(1))
                .Returns(this.result);
            this.Response = this.Client.Get(
                $"/purchasing/reports/bom-history?"
                + $"bomName=SK HUB&from={19.September(2023):o}&to={19.September(2023).AddDays(1):o}"
                + $"&includeSubAssemblies=true",
                with => { with.Accept("application/json"); }).Result;
        }

        [Test]
        public void ShouldCallDomain()
        {
            this.MockDomainService.Received().GetReportWithSubAssemblies(
                "SK HUB", 19.September(2023), 19.September(2023).AddDays(1));
        }
    }
}
