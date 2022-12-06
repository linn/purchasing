namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestServiceTests
{
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NSubstitute;

    public class WhenCancellingChangeRequestAndNotAuthorised : ContextBase
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            var request = new ChangeRequest()
                              {
                                  DocumentNumber = 1,
                                  ChangeState = "ACCEPT",
                                  DateEntered = new DateTime(2022, 1, 1),
                                  DescriptionOfChange = "Secret Change Request"
                              };
            this.Repository.FindById(1).Returns(request);

            this.action = () => this.Sut.Cancel(1, new List<string>());
        }

        [Test]
        public void ShouldThrowUnauthorisedActionException()
        {
            this.action.Should().Throw<UnauthorisedActionException>();
        }
    }
}
