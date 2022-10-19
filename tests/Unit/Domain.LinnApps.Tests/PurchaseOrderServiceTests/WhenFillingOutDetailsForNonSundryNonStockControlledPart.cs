﻿namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderServiceTests
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

    public class WhenFillingOutDetailsForNonSundryNonStockControlledPart : ContextBase
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
                NominalCode = "0000005432",
                Department = new Department
                {
                    DepartmentCode = "0000001234"
                },
                AccountId = 886
            };
            this.part = new Part
            {
                OurUnitOfMeasure = "ONES",
                PartNumber = "TELEPHONE",
                NominalAccount = this.nominal,
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
                        Currency = new Currency { Code = "GBP" }
                    });
            this.CurrencyPack.GetExchangeRate("GBP", "GBP").Returns(1);
            this.result = this.Sut.FillOutUnsavedOrder(this.args, 33087);
        }

        [Test]
        public void ShouldSetNullNominalAndDepartmentFromPart()
        {
            this.result.Details.First().OrderPosting.NominalAccount.NominalCode.Should()
                .Be(this.part.NominalAccount.NominalCode);
            this.result.Details.First().OrderPosting.NominalAccount.DepartmentCode.Should()
                .Be(this.part.NominalAccount.DepartmentCode);
        }
    }
}
