namespace Linn.Purchasing.Domain.LinnApps.Tests.BomVerificationHistoryServiceTests
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenCreatingBomHistoryVerificationEntryButInvalidEmployee : ContextBase
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            var request = new BomVerificationHistory
                              {
                                  TRef = 123,
                                  DateVerified= new DateTime(2023, 1, 1).ToString("o"),    
                                  DocumentNumber = 123456,
                                  DocumentType = "Test",
                                  PartNumber = "CAP 500",
                                  Remarks = "Big testing",
                                  VerifiedBy = 33087

        };
            this.action = () => this.Sut.CreateBomVerificationHistory(request);
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.action.Should().Throw<ItemNotFoundException>();
        }
    }
}
