namespace Linn.Purchasing.Domain.LinnApps.Tests.PlCreditDebitNoteServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreatingANote : ContextBase
    {
        private PlCreditDebitNote candidate;

        private PlCreditDebitNote result;

        [SetUp]
        public void SetUp()
        {
            var order = new PurchaseOrder 
                            {
                                Details = new List<PurchaseOrderDetail> 
                                            { 
                                                new PurchaseOrderDetail
                                                  {
                                                      Line = 1, SuppliersDesignation = "HELLO"
                                                  }
                                            }
                            };
            this.MockOrderRepository.FindById(123).Returns(order);
            var supplier = new Supplier { SupplierId = 123 };
            this.MockSupplierRepository.FindById(123).Returns(supplier);
            this.MockCurrencyRepository.FindById("GBP").Returns(new Currency());
            this.candidate = new PlCreditDebitNote 
                                 { 
                                     OriginalOrderNumber = 123,
                                     OriginalOrderLine = 1,
                                     NoteType = new CreditDebitNoteType { Type = "C" },
                                     Supplier = supplier,
                                     Currency = new Currency { Code = "GBP" },
                                     Details = new List<PlCreditDebitNoteDetail> 
                                                   { 
                                                       new PlCreditDebitNoteDetail 
                                                           {
                                                                LineNumber = 1
                                                           }
                                                   }
                                 };
            this.MockSalesTaxPack.GetVatRateSupplier(123).Returns(0.2m);
            this.MockAuthService.HasPermissionFor(
                AuthorisedAction.PlCreditDebitNoteCreate,
                Arg.Is<List<string>>(x => !x.Contains(AuthorisedAction.PlCreditDebitNoteCreate))).Returns(true);
            this.MockDatabaseService.GetNextVal("PLCDN_SEQ").Returns(666);
            this.result = this.Sut.CreateCreditNote(
                this.candidate,
                new List<string>());
        }

        [Test]
        public void ShouldReturnCreated()
        {
            this.result.NoteNumber.Should().Be(666);
            this.result.VatRate.Should().Be(0.2m);
            this.result.DateCreated.Should().Be(DateTime.Today);
            this.result.Details.First().NoteNumber.Should().Be(666);
        }
    }
}
