namespace Linn.Purchasing.Domain.LinnApps.Tests.PlCreditDebitNotesTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenClosingADebitNote : ContextBase
    {
        private PlCreditDebitNote note;

        [SetUp]
        public void SetUp()
        {
            this.note = new PlCreditDebitNote { DateCreated = DateTime.UnixEpoch, NoteNumber = 1 };
            this.MockAuthService.HasPermissionFor(
                AuthorisedAction.PlCreditDebitNoteClose,
                Arg.Is<List<string>>(x => x.Contains(AuthorisedAction.PlCreditDebitNoteClose))).Returns(true);

            this.Sut.CloseDebitNote(
                this.note,
                "REASON",
                33087,
                new List<string> { AuthorisedAction.PlCreditDebitNoteClose });
        }

        [Test]
        public void ShouldReturnClosed()
        {
            this.note.NoteNumber.Should().Be(1);
            this.note.DateClosed.Should().Be(DateTime.Today);
            this.note.ReasonClosed.Should().Be("REASON");
            this.note.ClosedBy.Should().Be(33087);
        }
    }
}
