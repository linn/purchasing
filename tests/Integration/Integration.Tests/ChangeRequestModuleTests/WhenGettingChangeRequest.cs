namespace Linn.Purchasing.Integration.Tests.ChangeRequestModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Edi;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingChangeRequest : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            var changeRequest = new ChangeRequest
                             {
                                 DocumentType = "CRF",
                                 DocumentNumber = 1,
                                 DateEntered = new DateTime(2020,1,1),
                                 ReasonForChange = "Everything changes",
                                 DescriptionOfChange = "Ch ch changes",
                                 ChangeState = "ACCEPT",
                                 BomChanges = new List<BomChange>
                                                  {
                                                      new BomChange
                                                          {
                                                              ChangeId = 1,
                                                              BomName = "TOAST 001",
                                                              ChangeState = "ACCEPT"
                                                          }
                                                  }
                             };
            this.Repository.FindById(1).Returns(changeRequest);

            this.Response = this.Client.Get(
                "/purchasing/change-requests/1",
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
            this.Repository.Received().FindById(1);
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var result = this.Response.DeserializeBody<ChangeRequestResource>();
            result.Should().NotBeNull();
            result.DocumentNumber.Should().Be(1);
            result.ChangeState.Should().Be("ACCEPT");
            result.BomChanges.Count().Should().Be(1);
        }
    }
}
