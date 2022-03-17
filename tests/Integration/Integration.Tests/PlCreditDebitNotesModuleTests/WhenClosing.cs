namespace Linn.Purchasing.Integration.Tests.PlCreditDebitNotesModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenClosing : ContextBase
    {
        private PlCreditDebitNoteResource resource;

        private PlCreditDebitNote note;

        [SetUp]
        public void SetUp()
        {
            this.resource = new PlCreditDebitNoteResource
                                {
                                    NoteNumber = 1,
                                    Close = true,
                                    ReasonClosed = "REASON",
                                };
            this.note = new PlCreditDebitNote
                            {
                                NoteNumber = this.resource.NoteNumber,
                                DateCreated = DateTime.UnixEpoch,
                                Supplier = new Supplier { SupplierId = 1 },
                                NoteType = new CreditDebitNoteType { Type = "C" }
                            };

            this.MockPlCreditDebitNoteRepository.FindById(1).Returns(this.note);

            this.Response = this.Client.Put(
                $"/purchasing/pl-credit-debit-notes/{this.resource.NoteNumber}",
                this.resource,
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
            var res = this.Response.DeserializeBody<PlCreditDebitNoteResource>();
            res.Should().NotBeNull();
        }

        [Test]
        public void ShouldCallCorrectDomainServiceMethod()
        {
            this.MockDomainService.Received().CloseDebitNote(
                Arg.Is<PlCreditDebitNote>(x => x.NoteNumber == this.resource.NoteNumber),
                this.resource.ReasonClosed,
                Arg.Any<int>(),
                Arg.Any<IEnumerable<string>>());

            this.MockDomainService.DidNotReceive().UpdatePlCreditDebitNote(
                Arg.Any<PlCreditDebitNote>(),
                Arg.Any<PlCreditDebitNote>(),
                Arg.Any<IEnumerable<string>>());

            this.MockDomainService.DidNotReceive().CancelDebitNote(
                Arg.Any<PlCreditDebitNote>(),
                Arg.Any<string>(),
                Arg.Any<int>(),
                Arg.Any<IEnumerable<string>>());
        }
    }
}
