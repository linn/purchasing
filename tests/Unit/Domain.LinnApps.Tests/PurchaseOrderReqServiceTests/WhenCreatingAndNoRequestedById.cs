namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderReqServiceTests
{
    using System;
    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrderReqs;

    using NUnit.Framework;

    public class WhenCreatingAndNoRequestedById : ContextBase
    {
        private Action action;

        private PurchaseOrderReq candidate;

        [SetUp]
        public void SetUp()
        {
            this.candidate = new PurchaseOrderReq
            {
                RequestedById = 0,
            };

            this.action = () => this.Sut.Create(candidate, null);
        }

        [Test]
        public void ShouldThrowException()
        {
            this.action.Should().Throw<PurchaseOrderReqException>()
                .WithMessage("Could not determine logged in user from request");
        }
    }
}
