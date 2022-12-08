namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenFillingOutOrderAndSupplierIsClosed : ContextBase
    {
        private PurchaseOrder args;

        private Part part;

        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.part = new Part
                            {
                                OurUnitOfMeasure = "ONES",
                                PartNumber = "SUNDRY"
                            };
            this.PartQueryRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>())
                .Returns(this.part);
            this.args = new PurchaseOrder
                            {
                                Supplier = new Supplier { SupplierId = 1 },
                                DeliveryAddress = new LinnDeliveryAddress(),
                                Details =
                                    new List<PurchaseOrderDetail>
                                        {
                                            new PurchaseOrderDetail
                                                {
                                                    Line = 1,
                                                    Part = this.part
                                                }
                                        }
                            };
            this.SupplierRepository
                .FindById(
                    this.args.Supplier.SupplierId).Returns(
                    new Supplier
                        {
                            OrderAddress = new Address(),
                            InvoiceFullAddress = new FullAddress(),
                            Currency = new Currency(),
                            DateClosed = DateTime.UnixEpoch
                    });

            this.action = () => this.Sut.FillOutUnsavedOrder(this.args, 33087);
        }

        [Test]
        public void ShouldThrowException()
        {
            this.action.Should().Throw<PurchaseOrderException>().WithMessage("Supplier is closed!");
        }
    }
}
