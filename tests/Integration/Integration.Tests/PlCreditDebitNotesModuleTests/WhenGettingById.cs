namespace Linn.Purchasing.Integration.Tests.PlCreditDebitNotesModuleTests
{
    using System;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingById : ContextBase
    {
        private PlCreditDebitNote data;
        [SetUp]
        public void SetUp()
        {
            this.data = new PlCreditDebitNote
                            {
                                NoteNumber = 1, 
                                Supplier = new Supplier { Name = "SUPPLIER", SupplierId = 123 },
                                PartNumber = "PART",
                                DateCreated = DateTime.UnixEpoch,
                                NetTotal = 100,
                                NoteType = new CreditDebitNoteType { Type = "C" },
                                Notes = "NOTES",
                                OrderQty = 456,
                                PurchaseOrder = new PurchaseOrder { OrderNumber = 4567 },
                                ReturnsOrderNumber = 4321
                            };
            this.MockPlCreditDebitNoteRepository.FindById(this.data.NoteNumber).Returns(this.data);

            this.Response = this.Client.Get(
                $"/purchasing/pl-credit-debit-notes/{this.data.NoteNumber}",
                with =>
                    {
                        with.Accept("application/json");
                    }).Result;
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldReturnJsonContentType()
        {
            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resource = this.Response.DeserializeBody<PlCreditDebitNoteResource>();
            resource.Should().NotBeNull();
            resource.NoteNumber.Should().Be(this.data.NoteNumber);
            resource.SupplierName.Should().Be(this.data.Supplier.Name);
            resource.SupplierId.Should().Be(this.data.Supplier.SupplierId);
            resource.PartNumber.Should().Be(this.data.PartNumber);
            resource.DateCreated.Should().Be(DateTime.UnixEpoch.ToString("o"));
            resource.NetTotal.Should().Be(this.data.NetTotal);
            resource.NoteType.Should().Be(this.data.NoteType.Type);
            resource.Notes.Should().Be(this.data.Notes);
            resource.OrderQty.Should().Be(this.data.OrderQty);
            resource.OriginalOrderNumber.Should().Be(this.data.PurchaseOrder.OrderNumber);
            resource.ReturnsOrderNumber.Should().Be(this.data.ReturnsOrderNumber);
        }
    }
}
