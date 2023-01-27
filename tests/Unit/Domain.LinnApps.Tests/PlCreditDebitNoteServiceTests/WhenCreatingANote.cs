namespace Linn.Purchasing.Domain.LinnApps.Tests.PlCreditDebitNoteServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreatingANote : ContextBase
    {
        private PlCreditDebitNote candidate;

        private PlCreditDebitNote result;

        [SetUp]
        public void SetUp()
        {
            this.candidate = new PlCreditDebitNote 
                                 { 
                                     NoteType = new CreditDebitNoteType { Type = "C" },
                                     Supplier = new Supplier { SupplierId = 123 },
                                     Details = new List<PlCreditDebitNoteDetail> 
                                                   { 
                                                       new PlCreditDebitNoteDetail 
                                                           {
                                                                LineNumber = 1
                                                           }
                                                   }
                                 };
            this.MockAuthService.HasPermissionFor(
                AuthorisedAction.PlCreditDebitNoteCreate,
                Arg.Is<List<string>>(x => !x.Contains(AuthorisedAction.PlCreditDebitNoteCreate))).Returns(true);
            this.MockDatabaseService.GetNextVal("PLCDN_SEQ").Returns(666);
            this.result = this.Sut.CreateCreditNote(
                this.candidate,
                new List<string>());
        }

        [Test]
        public void ShouldReturnCreated()
        {
            this.result.NoteNumber.Should().Be(666);
            this.result.DateCreated.Should().Be(DateTime.Today);
            this.result.Details.First().NoteNumber.Should().Be(666);
        }
    }
}
