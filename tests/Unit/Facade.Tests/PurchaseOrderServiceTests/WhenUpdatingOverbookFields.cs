namespace Linn.Purchasing.Facade.Tests.PurchaseOrderServiceTests
{
    using System.Collections.Generic;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingOverbookFields : ContextBase
    {
        private PurchaseOrder model;

        private PurchaseOrder updated;

        private IResult<PurchaseOrderResource> result;

        private PurchaseOrderResource toResource;

        private PurchaseOrderResource fromResource;


        [SetUp]
        public void SetUp()
        {
            this.fromResource = new PurchaseOrderResource
                                    {
                                        OrderNumber = 600179,
                                        Overbook = "N",
                                        OverbookQty = 0
                                    };

            this.toResource = new PurchaseOrderResource
                                  {
                                      OrderNumber = 600179,
                                      Overbook = "Y",
                                      OverbookQty = 1
                                  };

            this.model = new PurchaseOrder
                             {
                                 OrderNumber = 600179,
                                 Cancelled = string.Empty,
                                 OrderDate = 10.January(2021),
                                 Overbook = "Y",
                                 OverbookQty = 1,
                                 SupplierId = 1224,
                                 Supplier = new Supplier { SupplierId = 1224 },
                                 CurrencyCode = "GBP",
                                 OrderMethodName = "carrier pigeon",
                                 RequestedById = 111,
                                 EnteredById = 222,
            };

            this.updated = this.model;
            this.updated.OverbookQty = this.toResource.OverbookQty;
            this.updated.Overbook = this.toResource.Overbook;

            this.PurchaseOrderRepository.FindById(this.model.OrderNumber).Returns(this.model);

            this.DomainService.AllowOverbook(
                Arg.Is<PurchaseOrder>(p => p.OrderNumber == this.model.OrderNumber),
                this.toResource.Overbook,
                this.toResource.OverbookQty,
                Arg.Any<IEnumerable<string>>())
                .Returns(this.model);

            this.AuthService.HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, Arg.Any<IEnumerable<string>>())
                .Returns(true);
            this.result = this.Sut.PatchOrder(
                new PatchRequestResource<PurchaseOrderResource>
                    {
                        From = this.fromResource,
                        To = this.toResource
                    },
                33111, 
                new List<string>());
        }

        [Test]
        public void ShouldBuildResource()
        {
            this.result.Should().BeOfType<SuccessResult<PurchaseOrderResource>>();
            var dataResult = ((SuccessResult<PurchaseOrderResource>)this.result).Data;
        }

        [Test]
        public void ShouldCallOverbookDomainMethod()
        {
            this.DomainService.Received().AllowOverbook(
                Arg.Any<PurchaseOrder>(),
                this.model.Overbook,
                this.model.OverbookQty,
                Arg.Any<List<string>>());
        }

        [Test]
        public void ShouldAddToOverbookLogRepo()
        {
            this.OverbookAllowedByLogRepository.Received().Add(Arg.Any<OverbookAllowedByLog>());
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.result.Should().BeOfType<SuccessResult<PurchaseOrderResource>>();
            var dataResult = ((SuccessResult<PurchaseOrderResource>)this.result).Data;
            dataResult.OrderNumber.Should().Be(this.model.OrderNumber);
            dataResult.Cancelled.Should().Be(this.model.Cancelled);
            dataResult.OrderDate.Should().Be(this.model.OrderDate.ToString("O"));
            dataResult.Overbook.Should().Be(this.model.Overbook);
            dataResult.OverbookQty.Should().Be(this.model.OverbookQty);
            dataResult.Supplier.Id.Should().Be(this.model.SupplierId);
        }
    }
}
