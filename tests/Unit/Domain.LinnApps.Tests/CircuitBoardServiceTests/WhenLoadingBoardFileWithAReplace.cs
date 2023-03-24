namespace Linn.Purchasing.Domain.LinnApps.Tests.CircuitBoardServiceTests
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Parts;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenLoadingBoardFileWithAReplace : ContextBase
    {
        private ProcessResult result;

        private string revision;

        private string file;

        [SetUp]
        public void SetUp()
        {
            this.revision = "L1R1";
            this.file = @"Designator	Part No	Part Description	Footprint	Tolerance	Negative Tolerance	Positive Tolerance	Technology	Value	Voltage	DIELECTRIC	Qty

""BR100""	""MISS266""	""D15XB60H 15A 600V BRIDGE DIODE SINGLE IN LINE PACKAGE""	""D15XBXXHV""	""""	""""	""""	""TH""	""""	""""	""""	""""
""C002""	""RES966""	""RES""	""BBBB""	""""	""""	""""	""TH""	""""	""""	""""	""""";

            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>()).Returns(new Part());

            this.result = this.Sut.UpdateFromFile(
                this.BoardCode,
                this.revision,
                "TSB",
                this.file,
                this.PcasChange,
                true);
        }

        [Test]
        public void ShouldUpdateBoard()
        {
            this.Board.Components.Should().HaveCount(3);
            var added = this.Board.Components.First(a => a.BoardLine == 2);
            added.PartNumber.Should().Be("MISS 266");
            added.Quantity.Should().Be(1);
        }

        [Test]
        public void ShouldMakeSuggestedChangesMessage()
        {
            this.result.Message.Should().Contain("THE FOLLOWING CHANGES HAVE BEEN MADE FOR BOARD 123 revision L1R1");
            this.result.Message.Should().Contain("Pcb part number on revision is  but found  in the file.");
            this.result.Message.Should().Contain("Adding MISS 266 at BR100.");
            this.result.Message.Should().Contain("Replacing CAP 123 with RES 966 at C002");
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.result.Success.Should().BeTrue();
        }
    }
}
