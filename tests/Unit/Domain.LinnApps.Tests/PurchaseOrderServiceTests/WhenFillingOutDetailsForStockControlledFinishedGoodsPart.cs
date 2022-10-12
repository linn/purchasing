namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenFillingOutDetailsForStockControlledFinishedGoodsPart : ContextBase
    {
        private PurchaseOrder args;

        private PurchaseOrder result;

        private Part part;

        private NominalAccount nominal;

        [SetUp]
        public void SetUp()
        {
            this.nominal = new NominalAccount
            {
                NominalCode = "0000007617",
                Department = new Department
                {
                    DepartmentCode = "0000002508"
                },
                AccountId = 886
            };
            this.part = new Part
            {
                OurUnitOfMeasure = "ONES",
                StockControlled = "Y",
                RawOrFinished = "F"
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
                        Currency = new Currency()
                    });
            this.NominalAccountRepository
                .FindBy(Arg.Any<Expression<Func<NominalAccount, bool>>>()).Returns(this.nominal);
            
            this.result = this.Sut.FillOutUnsavedOrder(this.args, 33087);
        }

        [Test]
        public void ShouldSelectCorrectNominalAndDepartment()
        {
            this.result.Details
                .First().OrderPosting.NominalAccount.NominalCode
                .Should().Be(this.nominal.NominalCode);
            this.result.Details
                .First().OrderPosting.NominalAccount.Department.DepartmentCode
                .Should().Be(this.nominal.Department.DepartmentCode);
        }
    }
}

