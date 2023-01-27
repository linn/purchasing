namespace Linn.Purchasing.Integration.Tests.PlCreditDebitNotesModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreating : ContextBase
    {
        private PlCreditDebitNoteResource resource;

        private PlCreditDebitNote note;

        [SetUp]
        public void SetUp()
        {
            this.resource = new PlCreditDebitNoteResource
                                {
                                    Links = new LinkResource[] { },
                                    NoteType = "C", 
                                    PartNumber = "CAP 500",
                                    OrderQty = 1,
                                    OriginalOrderNumber = 123,
                                    ReturnsOrderNumber = null,
                                    NetTotal = 12,
                                    Notes = "NOTE",
                                    SupplierId = 123,
                                    OrderUnitPrice = 12,
                                    OrderUnitOfMeasure = "ONES",
                                    VatTotal = 2.4m,
                                    SupplierFullAddress = null,
                                    Total = 14.4m
                                };
            this.note = new PlCreditDebitNote
            {
                NoteNumber = this.resource.NoteNumber,
                DateCreated = DateTime.UnixEpoch,
                Supplier = new Supplier { SupplierId = 1 },
                NoteType = new CreditDebitNoteType { Type = "C" }
            };

            this.MockDomainService.CreateCreditNote(Arg.Any<PlCreditDebitNote>(), Arg.Any<IEnumerable<string>>())
                .Returns(this.note);

            this.Response = this.Client.PostAsJsonAsync(
                $"/purchasing/pl-credit-debit-notes",
                this.resource).Result;
        }

        [Test]
        public void ShouldReturnCreated()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.Created);
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
            var res = this.Response.DeserializeBody<PlCreditDebitNoteResource>();
            res.Should().NotBeNull();
        }

        [Test]
        public void ShouldCallCorrectDomainServiceMethod()
        {
            this.MockDomainService.Received().CreateCreditNote(
                Arg.Is<PlCreditDebitNote>(x => x.OriginalOrderNumber == this.resource.OriginalOrderNumber),
                Arg.Any<IEnumerable<string>>());
        }
    }
}
