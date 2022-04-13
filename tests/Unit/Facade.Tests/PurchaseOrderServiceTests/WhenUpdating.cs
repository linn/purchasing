namespace Linn.Purchasing.Facade.Tests.PurchaseOrderServiceTests
{
    using System.Collections.Generic;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdating : ContextBase
    {
        private PurchaseOrderResource updateResource;

        private IResult<PurchaseOrderResource> result;

        [SetUp]
        public void SetUp()
        {
            this.Builder.Build(Arg.Any<PurchaseOrder>(), Arg.Any<IEnumerable<string>>())
                .Returns(new PurchaseOrderResource
                             {
                                 OrderNumber = 600179,
                                 Cancelled = string.Empty,
                                 DocumentType = string.Empty,
                                 DateOfOrder = 10.January(2021),
                                 Overbook = "Y",
                                 OverbookQty = 1,
                                 SupplierId = 1224
                });

            var purchaseOrder = new PurchaseOrder
                                    {
                                        OrderNumber = 600179,
                                        Cancelled = string.Empty,
                                        DocumentTypeName = string.Empty,
                                        OrderDate = 10.January(2021),
                                        Overbook = string.Empty,
                                        OverbookQty = 0,
                                        SupplierId = 1224
                                    };

            this.updateResource = new PurchaseOrderResource()
            {
                OrderNumber = 600179,
                Cancelled = string.Empty,
                DocumentType = string.Empty,
                DateOfOrder = 10.January(2021),
                Overbook = "Y",
                OverbookQty = 1,
                SupplierId = 1224
            };

            this.PurchaseOrderRepository.FindById(600179).Returns(purchaseOrder);

            this.result = this.Sut.Update(this.updateResource.OrderNumber, this.updateResource);
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.result.Should().BeOfType<SuccessResult<PurchaseOrderResource>>();
            var dataResult = ((SuccessResult<PurchaseOrderResource>)this.result).Data;
            dataResult.OrderNumber.Should().Be(600179);
            dataResult.Overbook.Should().Be("Y");
            dataResult.OverbookQty.Should().Be(1);
        }
    }
}
