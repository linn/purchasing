﻿namespace Linn.Purchasing.Domain.LinnApps.Tests.PlCreditDebitNotesTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCancellingADebitNoteAndUserNotAuthorised : ContextBase
    {
        private Action action;

        private PlCreditDebitNote note;

        [SetUp]
        public void SetUp()
        {
            this.note = new PlCreditDebitNote { DateCreated = DateTime.UnixEpoch, NoteNumber = 1 };
            this.MockAuthService.HasPermissionFor(
                AuthorisedAction.PlCreditDebitNoteCancel,
                Arg.Is<List<string>>(x => !x.Contains(AuthorisedAction.PlCreditDebitNoteClose))).Returns(false);

            this.action = () => this.Sut.CancelDebitNote(
                this.note,
                "REASON",
                33087,
                new List<string>());
        }

        [Test]
        public void ShouldThrowUnauthorisedActionException()
        {
            this.action.Should().Throw<UnauthorisedActionException>();
        }
    }
}
