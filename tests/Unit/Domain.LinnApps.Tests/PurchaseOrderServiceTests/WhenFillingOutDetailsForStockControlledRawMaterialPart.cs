﻿namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenFillingOutDetailsForStockControlledRawMaterialPart : ContextBase
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
                NominalCode = "0000007635",
                Department = new Department
                {
                    DepartmentCode = "0000002508"
                },
                AccountId = 884
            };
            this.part = new Part
            {
                OurUnitOfMeasure = "ONES",
                StockControlled = "Y",
                RawOrFinished = "R",
                DrawingReference = "Rev 123"
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
                        DeliveryDay = "THURSDAY"
                    });
            this.NominalAccountRepository
                .FindBy(Arg.Any<Expression<Func<NominalAccount, bool>>>()).Returns(this.nominal);
            this.PartSupplierRepository.FindById(Arg.Any<PartSupplierKey>())
                .Returns(new PartSupplier { LeadTimeWeeks = 2 });
            this.result = this.Sut.FillOutUnsavedOrder(this.args, 33087);
        }

        [Test]
        public void ShouldSelectCorrectNominalAndDepartment()
        {
            this.result.Details
                .First().OrderPosting.NominalAccount.Department.DepartmentCode
                .Should().Be(this.nominal.Department.DepartmentCode);
            this.result.Details
                .First().OrderPosting.NominalAccount.NominalCode
                .Should().Be(this.nominal.NominalCode);
        }

        [Test]
        public void ShouldSetRequestedDateToBeOnSuppliersDeliveryDay()
        {
            this.result.Details.First().PurchaseDeliveries.First()
                .DateRequested.GetValueOrDefault().DayOfWeek.Should()
                .Be(DayOfWeek.Thursday);
        }

        [Test]
        public void ShouldSetDrawingReference()
        {
            this.result.Details.First().DrawingReference.Should().Be(this.part.DrawingReference);
        }
    }
}
