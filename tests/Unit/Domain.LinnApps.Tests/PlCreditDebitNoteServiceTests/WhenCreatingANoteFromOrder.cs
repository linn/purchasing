namespace Linn.Purchasing.Domain.LinnApps.Tests.PlCreditDebitNoteServiceTests
{
    using System.Collections.Generic;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreatingANoteFromOrder : ContextBase
    {
        private PurchaseOrder order;

        [SetUp]
        public void SetUp()
        {
            this.MockNoteTypeRepository.FindById("D").Returns(new CreditDebitNoteType { Type = "D" });
            this.MockSupplierRepository.FindById(1).Returns(new Supplier { SupplierId = 1 });
            this.order = new PurchaseOrder
                             {
                                 OrderNumber = 123,
                                 DocumentType = new DocumentType { Name = "CO" },
                                 DocumentTypeName = "CO",
                                 SupplierId = 1,
                                 EnteredById = 8,
                                 Details = new List<PurchaseOrderDetail>
                                               {
                                                   new PurchaseOrderDetail
                                                       {
                                                           OrderNumber = 123,
                                                           Line = 1,
                                                           OriginalOrderNumber = 456,
                                                           OriginalOrderLine = 11,
                                                           BaseOrderUnitPrice = 2,
                                                           BaseOurUnitPrice = 2,
                                                           OrderQty = 1,
                                                           OurQty = 1,
                                                           BaseDetailTotal = 2,
                                                           DetailTotalCurrency = 2,
                                                           OrderUnitPriceCurrency = 2,
                                                           OurUnitPriceCurrency = 2,
                                                           NetTotalCurrency = 2,
                                                           VatTotalCurrency = 0,
                                                           BaseVatTotal = 0,
                                                           BaseNetTotal = 2
                                                       }
                                               }
                             };

            this.Sut.CreateDebitOrCreditNoteFromPurchaseOrder(this.order);
        }

        [Test]
        public void ShouldAddNoteToRepository()
        {
            this.MockRepository.Received().Add(Arg.Any<PlCreditDebitNote>());
        }

        [Test]
        public void ShouldBeDebitNote()
        {
            this.MockRepository.Received().Add(
                Arg.Is<PlCreditDebitNote>(
                    a => a.NoteType.Type == "D" 
                         && a.OriginalOrderNumber == 456 
                         && a.OriginalOrderLine == 11
                         && a.CreditOrReplace == "CREDIT"));
        }
    }
}
