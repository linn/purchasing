namespace Linn.Purchasing.Integration.Tests.ChangeRequestModuleTests
{
    using FluentAssertions;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;
    using NSubstitute;
    using NUnit.Framework;

    using System.Net.Http.Json;
    using System.Net;
    using System;

    using Linn.Purchasing.Domain.LinnApps.Parts;
    using System.Linq.Expressions;

    public class WhenCreatingChangeRequest : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            var request = new ChangeRequestResource()
                              {
                                  EnteredBy = new EmployeeResource { Id = 1, FullName = "Matt Hancock" },
                                  ProposedBy = new EmployeeResource { Id = 2, FullName = "Bobby Davro" },
                                  NewPartNumber = "CONN 1",
                                  ReasonForChange = "Cos",
                                  DescriptionOfChange = "Ch Ch Changes"
                              };
            var part = new Part { PartNumber = "CONN 1", Description = "A Connector" };
            this.DatabaseService.GetNextVal("CRF_SEQ").Returns(42);
            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>()).Returns(part);

            this.Response = this.Client.PostAsJsonAsync(
                "/purchasing/change-requests", request).Result;
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Test]
        public void ShouldReturnJsonContentType()
        {
            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldCallAddRepository()
        {
            this.Repository.Received().Add(Arg.Any<ChangeRequest>());
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var result = this.Response.DeserializeBody<ChangeRequestResource>();
            result.Should().NotBeNull();
            result.DocumentNumber.Should().Be(42);
            result.ChangeState.Should().Be("PROPOS");
        }
    }
}
