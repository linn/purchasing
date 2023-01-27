namespace Linn.Purchasing.Domain.LinnApps.Tests.PlCreditDebitNoteServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreatingNoteAndNotAuthorised : ContextBase
    {
        private Action action;

        private PlCreditDebitNote note;

        [SetUp]
        public void SetUp()
        {
            this.note = new PlCreditDebitNote { DateCreated = DateTime.UnixEpoch, NoteNumber = 1 };
            this.MockAuthService.HasPermissionFor(
                AuthorisedAction.PlCreditDebitNoteCancel,
                Arg.Is<List<string>>(x => !x.Contains(AuthorisedAction.PlCreditDebitNoteCreate))).Returns(false);

            this.action = () => this.Sut.CreateCreditNote(
                this.note,
                new List<string>());
        }

        [Test]
        public void ShouldThrowUnauthorisedActionException()
        {
            this.action.Should().Throw<UnauthorisedActionException>();
        }
    }
}
