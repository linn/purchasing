namespace Linn.Purchasing.Integration.Tests.PartsReceivedReportModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Reports.Models;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingJobrefs : ContextBase
    {
        private List<TqmsJobref> dataResult;

        [SetUp]
        public void SetUp()
        {
            this.dataResult = new List<TqmsJobref>
                                  {
                                      new TqmsJobref
                                          {
                                              JobRef = "AA",
                                              Date = DateTime.UnixEpoch
                                          },
                                      new TqmsJobref
                                          {
                                              JobRef = "BB",
                                              Date = DateTime.UnixEpoch
                                          }
                                  };

            this.MockRepository.FindAll().Returns(this.dataResult.AsQueryable());
            
            this.Response = this.Client.Get(
                $"/purchasing/tqms-jobrefs",
                with =>
                    {
                        with.Accept("application/json");
                    }).Result;
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resources = this.Response.DeserializeBody<IEnumerable<TqmsJobRefResource>>()?.ToArray();
            resources.Should().NotBeNull();
            resources.Should().HaveCount(2);

            resources.First().JobRef.Should().Be("AA");
            resources.Last().JobRef.Should().Be("BB");
        }
    }
}
