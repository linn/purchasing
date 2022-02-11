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

    public class WhenNoRelevantPrivileges : ContextBase
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
                                 PackagingGroup = new PackagingGroup { Description = "GRP", Id = 555 },
                                 CreatedBy = new Employee { Id = 345, FullName = "EMPLOYEE" },
                                 DeliveryFullAddress = new FullAddress { Id = 1, AddressString = "FULL ADDRESS" },
                                 Manufacturer = new Manufacturer { Name = "MANUFACTURER", Code = "MAN" },
                                 Tariff = new Tariff { Code = "T", Description = "TARIFF"},
                                 OrderMethod = new OrderMethod { Name = "M", Description = "METHOD"},
                             };
            this.AuthService.HasPermissionFor(Arg.Any<string>(), Arg.Any<IEnumerable<string>>()).Returns(false);
            this.result = (PartSupplierResource)this.Sut.Build(this.model, new List<string>());
        }

        [Test]
        public void ShouldBuildCorrectResourceWithLinks()
        {
            this.result.SupplierId.Should().Be(this.model.SupplierId);
            this.result.SupplierName.Should().Be(this.model.Supplier.Name);
            this.result.CreatedBy.Should().Be(this.model.CreatedBy.Id);
            this.result.CreatedByName.Should().Be(this.model.CreatedBy.FullName);
            this.result.AddressId.Should().Be(this.model.DeliveryFullAddress.Id);
            this.result.FullAddress.Should().Be(this.model.DeliveryFullAddress.AddressString);
            this.result.ManufacturerCode.Should().Be(this.model.Manufacturer.Code);
            this.result.PackingGroup.Should().Be(this.model.PackagingGroup.Id);
            this.result.PackingGroupDescription.Should().Be(this.model.PackagingGroup.Description);
            this.result.TariffCode.Should().Be(this.model.Tariff.Code);
            this.result.TariffDescription.Should().Be(this.model.Tariff.Description);
            this.result.OrderMethodName.Should().Be(this.model.OrderMethod.Name);
            this.result.OrderMethodDescription.Should().Be(this.model.OrderMethod.Description);
            this.result.Links.Length.Should().Be(3);
            this.result.Links.SingleOrDefault(x => x.Rel == "self")?.Href.Should()
                .Be("/purchasing/part-suppliers/record?partId=123&supplierId=1");
            this.result.Links.SingleOrDefault(x => x.Rel == "part")?.Href.Should()
                .Be("/parts/123");
            this.result.Links.SingleOrDefault(x => x.Rel == "suppliers")?.Href.Should()
                .Be("/purchasing/suppliers/1");
        }
    }
}
