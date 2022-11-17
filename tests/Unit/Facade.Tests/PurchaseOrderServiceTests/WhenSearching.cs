namespace Linn.Purchasing.Facade.Tests.PurchaseOrderServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSearching : ContextBase
    {
        private IResult<IEnumerable<PurchaseOrderResource>> result;

        [SetUp]
        public void SetUp()
        {
            var purchaseOrders = new List<PurchaseOrder>
                                     {
                                         new PurchaseOrder
                                             {
                                                 OrderNumber = 600179,
                                                 Supplier = new Supplier { SupplierId = 118 }
                                             }
                                     };
            this.PurchaseOrderRepository.FilterBy(Arg.Any<Expression<Func<PurchaseOrder, bool>>>())
                .Returns(purchaseOrders.AsQueryable());
            this.AuthService.HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, Arg.Any<IEnumerable<string>>()).Returns(true);
            this.result = this.Sut.Search("600179", new List<string>());
        }

        [Test]
        public void ShouldCallSearch()
        {
            this.PurchaseOrderRepository.Received().FilterBy(Arg.Any<Expression<Func<PurchaseOrder, bool>>>());
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.result.Should().BeOfType<SuccessResult<IEnumerable<PurchaseOrderResource>>>();
            var dataResult = ((SuccessResult<IEnumerable<PurchaseOrderResource>>)this.result).Data.ToList();
            dataResult.FirstOrDefault(x => x.OrderNumber == 600179).Should().NotBeNull();
            dataResult.Count.Should().Be(1);
        }
    }
}
