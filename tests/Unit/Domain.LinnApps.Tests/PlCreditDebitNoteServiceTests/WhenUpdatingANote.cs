namespace Linn.Purchasing.Domain.LinnApps.Tests.PlCreditDebitNoteServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingANote : ContextBase
    {
        private PlCreditDebitNote note;

        private PlCreditDebitNote updated;

        [SetUp]
        public void SetUp()
        {
            this.note = new PlCreditDebitNote { DateCreated = DateTime.UnixEpoch, NoteNumber = 1 };
            this.updated = new PlCreditDebitNote { Notes = "NOTES" };

            this.MockAuthService.HasPermissionFor(
                AuthorisedAction.PlCreditDebitNoteUpdate,
                Arg.Is<List<string>>(x => x.Contains(AuthorisedAction.PlCreditDebitNoteUpdate))).Returns(true);

            this.Sut.UpdatePlCreditDebitNote(
                this.note,
                this.updated,
                new List<string> { AuthorisedAction.PlCreditDebitNoteUpdate });
        }

        [Test]
        public void ShouldReturnClosed()
        {
            this.note.NoteNumber.Should().Be(this.note.NoteNumber);
            this.note.Notes.Should().Be(this.updated.Notes);
        }
    }
}
