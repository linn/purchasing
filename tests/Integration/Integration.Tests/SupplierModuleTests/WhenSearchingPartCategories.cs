namespace Linn.Purchasing.Integration.Tests.SupplierModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSearchingPartCategories : ContextBase
    {
        private string searchTerm;

        private List<PartCategory> dataResult;

        [SetUp]
        public void SetUp()
        {
            this.searchTerm = "CAT";

            this.dataResult = new List<PartCategory>
                                  {
                                      new PartCategory
                                          {
                                              Category = "CAT", Description = "KITTEN"
                                          }
                                  };

            this.MockPartCategoriesRepository.FilterBy(Arg.Any<Expression<Func<PartCategory, bool>>>())
                .Returns(this.dataResult.AsQueryable());

            this.Response = this.Client.Get(
                $"/purchasing/part-categories?searchTerm={this.searchTerm}",
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
        public void ShouldReturnJsonContentType()
        {
            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resources = this.Response.DeserializeBody<IEnumerable<PartCategoryResource>>()?.ToArray();
            resources.Should().NotBeNull();
            resources.Should().HaveCount(1);
        }

        [Test]
        public void ShouldBuildResource()
        {
            var resource = this.Response.DeserializeBody<IEnumerable<PartCategoryResource>>()?.ToArray()?.First();
            resource.Category.Should().Be("CAT");
            resource.Description.Should().Be("KITTEN");
        }
    }
}
