﻿namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestServiceTests
{
    using FluentAssertions;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;

    using Linn.Common.Domain.Exceptions;

    public class WhenValidatingNoneExistantPart : ContextBase
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>()).Returns((Part)null);

            this.action = () => this.Sut.Approve(1, new List<string>());
        }

        [Test]
        public void ShouldThrowNotFoundException()
        {
            this.action.Should().Throw<DomainException>();
        }
    }
}
