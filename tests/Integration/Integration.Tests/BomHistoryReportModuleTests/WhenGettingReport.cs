namespace Linn.Purchasing.Integration.Tests.BomHistoryReportModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps.Reports.Models;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingReport : ContextBase
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
                + $"&includeSubAssemblies=false",
                with => { with.Accept("application/json"); }).Result;
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldCallDomain()
        {
            this.MockDomainService.Received().GetReport("SK HUB", 19.September(2023), 19.September(2023).AddDays(1));
        }

        [Test]
        public void ShouldReturnResult()
        {
            var resource = this.Response.DeserializeBody<IEnumerable<BomHistoryReportLineResource>>();
            resource.Should().NotBeNull();
            resource.First().BomName.Should().Be("SK HUB");
        }
    }
}
