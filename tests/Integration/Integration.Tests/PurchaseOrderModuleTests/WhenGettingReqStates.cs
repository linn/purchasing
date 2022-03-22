namespace Linn.Purchasing.Integration.Tests.PurchaseOrderModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingReqStates : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.PurchaseOrderReqStateFacadeService.GetAll().Returns(
                new SuccessResult<IEnumerable<PurchaseOrderReqStateResource>>(
                    new[]
                        {
                            new PurchaseOrderReqStateResource
                                {
                                    State = "authorised", Description = "authd", DisplayOrder = 2, IsFinalState = "Y"
                                },
                            new PurchaseOrderReqStateResource
                                {
                                    State = "created", Description = "made", DisplayOrder = 1, IsFinalState = "N"
                                }
                        }));

            this.Response = this.Client.Get(
                "/purchasing/purchase-orders/reqs/states",
                with => { with.Accept("application/json"); }).Result;
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resources = this.Response.DeserializeBody<IEnumerable<PurchaseOrderReqStateResource>>()?.ToArray();
            resources.Should().NotBeNull();
            resources.Should().HaveCount(2);

            resources?.First().State.Should().Be("authorised");
            resources.Second().State.Should().Be("created");
        }

        [Test]
        public void ShouldReturnJsonContentType()
        {
            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
