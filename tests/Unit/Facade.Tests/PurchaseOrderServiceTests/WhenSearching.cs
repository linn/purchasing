namespace Linn.Purchasing.Facade.Tests.PurchaseOrderServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSearching : ContextBase
    {
        private IResult<IEnumerable<PurchaseOrderResource>> result;

        [SetUp]
        public void SetUp()
        {
            this.Builder.Build(Arg.Any<PurchaseOrder>(), Arg.Any<IEnumerable<string>>())
                .Returns(new PurchaseOrderResource 
                    {
                        OrderNumber = 600179
                });  
            
            var purchaseOrders = new List<PurchaseOrder>
                                     {
                                         new PurchaseOrder
                                             {
                                                 OrderNumber = 600179
                                             }
                                     };

            this.PurchaseOrderRepository.FilterBy(Arg.Any<Expression<Func<PurchaseOrder, bool>>>())
                .Returns(purchaseOrders.AsQueryable());

             this.result = this.Sut.Search("600179");
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
