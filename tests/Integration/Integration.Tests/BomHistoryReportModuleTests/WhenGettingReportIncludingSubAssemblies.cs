namespace Linn.Purchasing.Integration.Tests.BomHistoryReportModuleTests
{
    using System;
    using System.Collections.Generic;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Integration.Tests.Extensions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingReportIncludingSubAssemblies : ContextBase
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
                + $"&includeSubAssemblies=true",
                with => { with.Accept("application/json"); }).Result;
        }

        [Test]
        public void ShouldCallDomain()
        {
            this.MockDomainService.Received().GetReportWithSubAssemblies(
                "SK HUB", DateTime.UnixEpoch, DateTime.UnixEpoch.AddDays(1));
        }
    }
}
