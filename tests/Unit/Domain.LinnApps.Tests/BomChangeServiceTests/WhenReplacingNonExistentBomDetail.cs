namespace Linn.Purchasing.Domain.LinnApps.Tests.BomChangeServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    using NUnit.Framework;

    public class WhenReplacingNonExistentBomDetail : ContextBase
    {
        private Action action;

        private ChangeRequest request;

        [SetUp]
        public void SetUp()
        {
            this.request = new ChangeRequest
                               {
                                   DocumentNumber = 1,
                                   ChangeState = "PROPOS",
                                   ChangeRequestType = "REPLACE",
                                   OldPart = new Part { PartNumber = "OLD 001" },
                                   NewPart = new Part { PartNumber = "NEW 002" }
                               };

            this.action = () => this.Sut.ReplaceBomDetail(1, this.request, 7004, 2);
        }

        [Test]
        public void ShouldThrowNotFoundException()
        {
            this.action.Should().Throw<ItemNotFoundException>();
        }
    }
}
