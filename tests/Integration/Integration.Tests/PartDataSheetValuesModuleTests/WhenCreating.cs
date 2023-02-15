namespace Linn.Purchasing.Integration.Tests.PartDataSheetValuesModuleTests
{
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreating : ContextBase
    {
        private PartDataSheetValuesResource resource;

        [SetUp]
        public void Setup()
        {
            this.resource = new PartDataSheetValuesResource
                                {
                                    AttributeSet = "ATT",
                                    Field = "F",
                                    Value = "VAL",
                                    Description = "DESC",
                                    AssemblyTechnology = "SM",
                                    ImdsNumber = 123,
                                    ImdsWeight = 456
                                };

            this.Response = this.Client.PostAsJsonAsync(
                $"/purchasing/part-data-sheet-values/",
                this.resource).Result;
        }

        [Test]
        public void ShouldReturnCreated()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Test]
        public void ShouldAdd()
        {
            this.PartDataSheetValuesRepository.Received().Add(Arg.Is<PartDataSheetValues>(
                x => x.Field == this.resource.Field
                     && x.AttributeSet == this.resource.AttributeSet
                     && x.AssemblyTechnology == this.resource.AssemblyTechnology
                     && x.Description == this.resource.Description
                     && x.Value == this.resource.Value
                     && x.ImdsNumber == this.resource.ImdsNumber
                     && x.ImdsWeight == this.resource.ImdsWeight));
        }
    }
}

