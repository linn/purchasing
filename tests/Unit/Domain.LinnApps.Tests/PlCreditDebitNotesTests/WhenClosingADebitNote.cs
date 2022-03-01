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

        private PlCreditDebitNote result;

        [SetUp]
        public void SetUp()
        {
            this.note = new PlCreditDebitNote { DateCreated = DateTime.UnixEpoch, NoteNumber = 1 };
            this.MockAuthService.HasPermissionFor(
                AuthorisedAction.PlCreditDebitNoteClose,
                Arg.Is<List<string>>(x => x.Contains(AuthorisedAction.PlCreditDebitNoteClose))).Returns(true);

            this.result = this.Sut.CloseDebitNote(
                this.note,
                "REASON",
                new List<string> { AuthorisedAction.PlCreditDebitNoteClose });
        }

        [Test]
        public void ShouldReturnClosed()
        {
            this.result.NoteNumber.Should().Be(1);
            this.result.DateClosed.Should().Be(DateTime.Today);
            this.result.ReasonClosed.Should().Be("REASON");
        }
    }
}
