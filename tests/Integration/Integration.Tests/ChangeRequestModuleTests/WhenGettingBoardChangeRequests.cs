namespace Linn.Purchasing.Integration.Tests.ChangeRequestModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;
    using NUnit.Framework;

    public class WhenGettingBoardChangeRequests : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Repository.FilterBy(Arg.Any<Expression<Func<ChangeRequest,bool>>>())
                .Returns(new List<ChangeRequest>
                             {
                                 new ChangeRequest
                                     {
                                         NewPartNumber = "SK HUB",
                                         DateEntered = DateTime.Today,
                                         ChangeState = "ACCEPT"
                                     }
                             }.AsQueryable());

            this.Response = this.Client.Get(
                "/purchasing/change-requests?searchTerm=SK HUB&includeForBoard=True",
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
            this.Repository.Received().FilterBy(Arg.Any<Expression<Func<ChangeRequest, bool>>>());
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var result = this.Response.DeserializeBody<IEnumerable<ChangeRequestResource>>().ToList();
            result.Should().NotBeNull();
            result.Count().Should().Be(1);
            result.First().NewPartNumber.Should().Be("SK HUB");
        }
    }
}
