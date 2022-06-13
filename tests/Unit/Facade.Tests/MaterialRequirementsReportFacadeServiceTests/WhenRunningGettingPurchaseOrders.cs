namespace Linn.Purchasing.Facade.Tests.MaterialRequirementsReportFacadeServiceTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Resources.MaterialRequirements;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenRunningGettingPurchaseOrders : ContextBase
    {
        private IEnumerable<string> privileges;

        private MrRequestResource requestResource;

        private IResult<MrPurchaseOrdersResource> result;

        private string jobRef;

        [SetUp]
        public void SetUp()
        {
            this.jobRef = "ABC";
            this.requestResource = new MrRequestResource
                                       {
                                           JobRef = this.jobRef,
                                           PartNumbers = new List<string> { "A", "B" }
                                       };
            this.privileges = new List<string>();

            this.MaterialRequirementsReportService.GetMaterialRequirementsOrders(
                    this.jobRef,
                    Arg.Is<IList<string>>(a => a.Contains("A") && a.Contains("B")))
                .Returns(
                    new List<MrPurchaseOrderDetail>
                        {
                            new MrPurchaseOrderDetail { OrderNumber = 1, Deliveries = new List<MrPurchaseOrderDelivery>() },
                            new MrPurchaseOrderDetail { OrderNumber = 2, Deliveries = new List<MrPurchaseOrderDelivery>() }
                        });

            this.result = this.Sut.GetMaterialRequirementOrders(this.requestResource, this.privileges);
        }

        [Test]
        public void ShouldCallService()
        {
            this.MaterialRequirementsReportService.Received().GetMaterialRequirementsOrders(
                this.jobRef, 
                Arg.Is<IList<string>>(a => a.Contains("A") && a.Contains("B")));
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.result.Should().BeOfType<SuccessResult<MrPurchaseOrdersResource>>();
            var data = ((SuccessResult<MrPurchaseOrdersResource>)this.result).Data.Orders.ToList();
            data.Should().HaveCount(2);
            data.Should().Contain(a => a.OrderNumber == 1);
            data.Should().Contain(a => a.OrderNumber == 2);
        }
    }
}
