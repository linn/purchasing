namespace Linn.Purchasing.Integration.Tests.ChangeRequestModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;
    using NUnit.Framework;

    public class WhenSearchingChangeRequests : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Repository.FindAll()
                .Returns(new List<ChangeRequest>
                             {
                                 new ChangeRequest
                                     {
                                         NewPartNumber = "SK HUB",
                                         DateEntered = DateTime.Today,
                                         ChangeState = "ACCEPT"
                                     },
                                 new ChangeRequest
                                     {
                                         NewPartNumber = "TURNTABLE",
                                         DateEntered = DateTime.Today,
                                         ChangeState = "ACCEPT"
                                     }
                             }.AsQueryable());

            this.Response = this.Client.Get(
                "/purchasing/change-requests?searchTerm=*HUB*",
                with => { with.Accept("application/json"); }).Result;
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldReturnJsonContentType()
        {
            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldCallRepository()
        {
            this.Repository.Received().FindAll();
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var result = this.Response.DeserializeBody<IEnumerable<ChangeRequestResource>>().ToList();
            result.Should().NotBeNull();
            result.Count.Should().Be(1);
            result.First().NewPartNumber.Should().Be("SK HUB");
        }
    }
}
