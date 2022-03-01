namespace Linn.Purchasing.Integration.Tests.PlCreditDebitNotesModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenClosing : ContextBase
    {
        private PlCreditDebitNoteResource resource;

        private PlCreditDebitNote note;

        private PlCreditDebitNote closed;

        [SetUp]
        public void SetUp()
        {
            this.resource = new PlCreditDebitNoteResource
                                {
                                    NoteNumber = 1,
                                    Close = true,
                                    ReasonClosed = "REASON"
                                };
            this.note = new PlCreditDebitNote
                            {
                                NoteNumber = this.resource.NoteNumber,
                                DateCreated = DateTime.UnixEpoch
                            };

            this.closed = new PlCreditDebitNote
                              {
                                  NoteNumber = this.resource.NoteNumber,
                                  DateClosed = DateTime.Today,
                                  ReasonClosed = this.resource.ReasonClosed,
                                  DateCreated = DateTime.UnixEpoch
                              };

            this.MockPlCreditDebitNoteRepository.FindById(1).Returns(this.note);

            this.MockDomainService.CloseDebitNote(this.note, this.resource.ReasonClosed, Arg.Any<IEnumerable<string>>())
                .Returns(this.closed);
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
    }
}
