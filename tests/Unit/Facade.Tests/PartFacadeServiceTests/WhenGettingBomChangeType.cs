namespace Linn.Purchasing.Facade.Tests.PartFacadeServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingBomChangeType : ContextBase
    {
        private IResult<BomTypeChangeResource> result;

        [SetUp]
        public void SetUp()
        {
            var part = new Part
                           {
                               Id= 1,
                               PartNumber = "CONN 1",
                               BomType = "A",
                               MaterialPrice = 2,
                               Description = "CONNECTOR",
                               Currency = new Currency { Code = "LZD", Name = "Liz Truss Dollars" }
                           };

            this.partRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>()).Returns(part);
            this.authorisationService.HasPermissionFor(Arg.Any<string>(), Arg.Any<IEnumerable<string>>()).Returns(true);

            this.result = this.Sut.GetBomType(1, new List<string>() { "change-bom-type" });
        }

        [Test]
        public void ShouldReturnSuccessRequest()
        {
            this.result.Should().BeOfType<SuccessResult<BomTypeChangeResource>>();
        }

        [Test]
        public void ShouldReturnBomTypeChangeWithPartInfo()
        {
            var resource = ((SuccessResult<BomTypeChangeResource>)this.result).Data;
            resource.PartNumber.Should().Be("CONN 1");
            resource.PartCurrency.Should().Be("LZD");
            resource.OldBomType.Should().Be("A");
            resource.OldSupplierId.Should().BeNull();
            resource.Links.Length.Should().Be(1);
        }
    }
}
