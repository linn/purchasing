namespace Linn.Purchasing.Facade.Tests.PartFacadeServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.Parts.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenChangingBomType : ContextBase
    {
        private IResult<BomTypeChangeResource> result;

        [SetUp]
        public void SetUp()
        {
            var part = new Part
                           {
                               PartNumber = "CONN 1", 
                               BomType = "A", 
                               MaterialPrice = 2, 
                               Description = "CONNECTOR", 
                               Currency = new Currency { Name = "Liz Truss Dollars" }
                           };

            this.partService.ChangeBomType(Arg.Any<BomTypeChange>(), Arg.Any<IEnumerable<string>>()).Returns(part);

            var request = new BomTypeChangeResource { PartNumber = "CONN 1", OldBomType = "C", NewBomType = "A" };
            this.result = this.Sut.ChangeBomType(request, new List<string>());
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
            resource.PartBomType.Should().Be("A");
            resource.PartCurrency.Should().Be("Liz Truss Dollars");
            resource.NewBomType.Should().Be("A");
        }
    }
}
