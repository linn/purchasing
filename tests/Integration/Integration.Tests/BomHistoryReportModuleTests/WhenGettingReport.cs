namespace Linn.Purchasing.Integration.Tests.BomHistoryReportModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingReport : ContextBase
    {
        private IEnumerable<BomHistoryViewEntry> result;

        [SetUp]
        public void SetUp()
        {
            this.result = new List<BomHistoryViewEntry>
                              {
                                  new BomHistoryViewEntry()
                                      {
                                          BomName = "SK HUB"
                                      }
                              };
            this.MockDomainService.GetReport("SK HUB", DateTime.UnixEpoch, DateTime.UnixEpoch.AddDays(1))
                .Returns(this.result);
            this.Response = this.Client.Get(
                $"/purchasing/reports/bom-history?"
                + $"bomName=SK HUB&from={DateTime.UnixEpoch:o}&to={DateTime.UnixEpoch.AddDays(1):o}"
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
            this.MockDomainService.Received().GetReport("SK HUB", DateTime.UnixEpoch, DateTime.UnixEpoch.AddDays(1));
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
