namespace Linn.Purchasing.Integration.Tests.PartDataSheetValuesModuleTests
{
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdating : ContextBase
    {
        private PartDataSheetValuesResource resource;

        private PartDataSheetValues entity;

        [SetUp]
        public void Setup()
        {
            this.entity = new PartDataSheetValues
                              {
                                  AttributeSet = "ATT",
                                  Field = "F",
                                  Value = "VAL",
                                  Description = "DESC",
                                  AssemblyTechnology = "SM",
                                  ImdsNumber = 123,
                                  ImdsWeight = 456
                              };
            this.resource = new PartDataSheetValuesResource
                                {
                                    AttributeSet = "ATT",
                                    Field = "F",
                                    Value = "VAL",
                                    Description = "DESC",
                                    AssemblyTechnology = "SM",
                                    ImdsNumber = 246,
                                    ImdsWeight = 912
                                };
            this.PartDataSheetValuesRepository
                .FindById(
                    Arg.Is<PartDataSheetValuesKey>(
                        k => k.AttributeSet == this.resource.AttributeSet && k.Field == this.resource.Field
                                                                          && k.Value == this.resource.Value))
                .Returns(this.entity);
            this.Response = this.Client.PutAsJsonAsync(
                $"/purchasing/part-data-sheet-values/ATT/F/VAL",
                this.resource).Result;
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldFindById()
        {
            this.PartDataSheetValuesRepository.Received().FindById(
                Arg.Is<PartDataSheetValuesKey>(
                    k => k.AttributeSet == this.resource.AttributeSet
                         && k.Field == this.resource.Field
                         && k.Value == this.resource.Value));
        }

        [Test]
        public void ShouldReturnJsonContentType()
        {
            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resultResource = this.Response.DeserializeBody<PartDataSheetValues>();
            resultResource.Should().NotBeNull();
            resultResource.Field.Should().Be("F");
        }

        [Test]
        public void ShouldUpdate()
        {
            this.entity.ImdsNumber.Should().Be(this.resource.ImdsNumber);
            this.entity.ImdsWeight.Should().Be(this.resource.ImdsWeight);
        }
    }
}
