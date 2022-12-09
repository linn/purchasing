namespace Linn.Purchasing.Domain.LinnApps.Tests.PcasChangeTests
{
    using System;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NUnit.Framework;

    public class WhenProposedAndGeneratingLifecycleText : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut = new PcasChange
                           {
                               DocumentNumber = 1,
                               ChangeState = "PROPOS",
                               DateEntered = new DateTime(2023, 2, 1),
                               EnteredBy = new Employee { Id = 3, FullName = "Baroness Ent" }
                           };
        }

        [Test]
        public void ShouldGenerateRightText()
        {
            this.Sut.LifecycleText().Should().Be("Created on 01-Feb-23 by Baroness Ent");
        }
    }
}
