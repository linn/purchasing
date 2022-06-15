﻿namespace Linn.Purchasing.Domain.LinnApps.Tests.EdiOrderServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Edi;

    using NSubstitute;

    using NUnit.Framework;

    internal class WhenSendingEdiEmail : ContextBase
    {
        private ProcessResult result;

        [SetUp]
        public void SetUp()
        {
            this.MockEdiEmailPack.SendEdiOrder(1, string.Empty, string.Empty, string.Empty, false)
                .Returns("SUCCESS test");

            this.result = this.Sut.SendEdiOrder(1, string.Empty, string.Empty, string.Empty, false);
        }

        [Test]
        public void ShouldReturnProcessResult()
        {
            this.result.Should().NotBeNull();
            this.result.Success.Should().BeTrue();
            this.result.Message.Should().Be("SUCCESS test");
        }
    }
}
