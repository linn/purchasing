namespace Linn.Purchasing.Domain.LinnApps.Tests.PlCreditDebitNotesTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCancellingADebitNote : ContextBase
    {
        private PlCreditDebitNote note;

        [SetUp]
        public void SetUp()
        {
            this.note = new PlCreditDebitNote { DateCreated = DateTime.UnixEpoch, NoteNumber = 1 };
            this.MockAuthService.HasPermissionFor(
                AuthorisedAction.PlCreditDebitNoteCancel,
                Arg.Is<List<string>>(x => x.Contains(AuthorisedAction.PlCreditDebitNoteCancel))).Returns(true);

            this.Sut.CancelDebitNote(
                this.note,
                "REASON",
                33087,
                new List<string> { AuthorisedAction.PlCreditDebitNoteCancel });
        }

        [Test]
        public void ShouldReturnCancelled()
        {
            this.note.NoteNumber.Should().Be(1);
            this.note.DateCancelled.Should().Be(DateTime.Today);
            this.note.ReasonCancelled.Should().Be("REASON");
            this.note.CancelledBy.Should().Be(33087);
        }
    }
}