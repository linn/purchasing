namespace Linn.Purchasing.Domain.LinnApps.Tests.PlCreditDebitNotesTests
{
    using System.IO;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSendingEmailAndNoOrderContactFound : ContextBase
    {
        private ProcessResult result;

        [SetUp]
        public void SetUp()
        {
            this.result = this.Sut.SendEmails(new Employee(), new PlCreditDebitNote(), null);
        }

        [Test]
        public void ShouldReturnFailResult()
        {
            this.result.Success.Should().Be(false);
            this.result.Message.Should().Be("Supplier has no main order contact");
        }
    }
}
