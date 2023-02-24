namespace Linn.Purchasing.Domain.LinnApps.Tests.CircuitBoardServiceTests
{
    using System;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Parts;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCheckingBoardFileAndInvalidCRef : ContextBase
    {
        private ProcessResult result;

        private string revision;

        private string file;

        [SetUp]
        public void SetUp()
        {
            this.revision = "L1R1";
            this.file = @"Designator	Part No	Part Description	Footprint	Tolerance	Negative Tolerance	Positive Tolerance	Technology	Value	Voltage	DIELECTRIC	Qty

""""	""MISS266""	""D15XB60H 15A 600V BRIDGE DIODE SINGLE IN LINE PACKAGE""	""D15XBXXHV""	""""	""""	""""	""TH""	""""	""""	""""	""""";


            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>())
                .Returns(new Part());
            this.result = this.Sut.UpdateFromFile(
                this.BoardCode,
                this.revision,
                "TSB",
                this.file,
                this.PcasChange,
                false);
        }

        [Test]
        public void ShouldMakeSuggestedChangesMessage()
        {
            this.result.Message.Should().Contain("Changes proposed (not made) for board 123 revision L1R1");
            this.result.Message.Should().Contain("Pcb part number on revision is  but found  in the file.");
            this.result.Message.Should().Contain("Adding MISS 266 at .");
            this.result.Message.Should().Contain("******* ERROR \"\" is not a valid Cref. Part on file line was MISS 266  *******");
            this.result.Message.Should().Contain("Removing CAP 123 from C002.");
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.result.Success.Should().BeTrue();
        }
    }
}
