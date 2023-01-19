namespace Linn.Purchasing.Integration.Tests.ChangeRequestModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;
    using System.Net.Http.Json;
    using System.Text;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingChangeRequest : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            var updatedResource = new ChangeRequestResource
                              {
                                  DocumentNumber = 1,
                                  EnteredBy = new EmployeeResource { Id = 1, FullName = "Matt Hancock" },
                                  ProposedBy = new EmployeeResource { Id = 2, FullName = "Bobby Davro" },
                                  NewPartNumber = "CONN 1",
                                  ReasonForChange = "Santa Claus is coming to town",
                                  DescriptionOfChange = "On a one horse open sleigh"
            };
            var request = new ChangeRequest
                              {
                                  DocumentNumber = 1,
                                  ChangeState = "PROPOS",
                                  ReasonForChange = "Salt",
                                  DescriptionOfChange = "Bae"
                              };
            this.Repository.FindById(1).Returns(request);

            this.Response = this.Client.PutAsJsonAsync(
                "/purchasing/change-requests/1", updatedResource).Result;
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
        public void ShouldCallCommit()
        {
            this.TransactionManager.Received().Commit();
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var result = this.Response.DeserializeBody<ChangeRequestResource>();
            result.Should().NotBeNull();
            result.DocumentNumber.Should().Be(1);
            result.ChangeState.Should().Be("PROPOS");
            result.ReasonForChange.Should().Be("Santa Claus is coming to town");
            result.DescriptionOfChange.Should().Be("On a one horse open sleigh");
        }
    }
}
