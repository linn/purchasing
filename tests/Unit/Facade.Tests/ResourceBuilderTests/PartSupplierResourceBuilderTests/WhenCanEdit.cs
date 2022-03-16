namespace Linn.Purchasing.Facade.Tests.ResourceBuilderTests.PartSupplierResourceBuilderTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCanEdit : ContextBase
    {
        private PartSupplier model;
        private PartSupplierResource result;

        [SetUp]
        public void SetUp()
        {
            this.model = new PartSupplier
                             {
                                 Part = new Part { PartNumber = "PART", Description = "DESCRIPTION", Id = 123 },
                                 SupplierId = 1,
                                 Supplier = new Supplier { Name = "SUPPLIER" },
                             };

            this.AuthService.HasPermissionFor(AuthorisedAction.PartSupplierUpdate, Arg.Any<IEnumerable<string>>()).Returns(true);

            this.result = (PartSupplierResource)this.Sut.Build(this.model, new List<string>());
        }

        [Test]
        public void ShouldBuildCorrectResourceWithLinks()
        {
            this.result.Links.Length.Should().Be(4);
            var editHref = this.result.Links.SingleOrDefault(x => x.Rel == "edit")?.Href;
            editHref.Should()
                .Be("/purchasing/part-suppliers/record?partId=123&supplierId=1");
        }
    }
}
