namespace Linn.Purchasing.Domain.LinnApps.Tests.EdiOrderServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Edi;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingEdiOrders : ContextBase
    {
        private IEnumerable<EdiOrder> result;

        [SetUp]
        public void SetUp()
        {
            var orders = new List<EdiOrder>
                             {
                                 new EdiOrder { Id = 1, OrderNumber = 1, SupplierId = 1 },
                                 new EdiOrder { Id = 2, OrderNumber = 2, SupplierId = 1 },
                                 new EdiOrder { Id = 3, OrderNumber = 3, SupplierId = 1 }
                             };

            this.MockEdiOrderRepository.FilterBy(Arg.Any<Expression<Func<EdiOrder, bool>>>())
                .Returns(orders.AsQueryable());

            this.result = this.Sut.GetEdiOrders(1);
        }

        [Test]
        public void ShouldReturnOrders()
        {
            this.result.Count().Should().Be(3);
        }
    }
}
