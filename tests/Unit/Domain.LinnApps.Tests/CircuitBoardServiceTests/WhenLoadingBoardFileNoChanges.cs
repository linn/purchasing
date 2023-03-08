namespace Linn.Purchasing.Domain.LinnApps.Tests.CircuitBoardServiceTests
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Parts;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenLoadingBoardFileNoChanges : ContextBase
    {
        private ProcessResult result;

        private string revision;

        private string file;

        [SetUp]
        public void SetUp()
        {
            this.revision = "L1R1";
            this.Board.Layouts.First().Revisions.First().PcbPartNumber = "PCB 1012";
            this.file = @"Designator	Part No	Part Description	Footprint	Tolerance	Negative Tolerance	Positive Tolerance	Technology	Value	Voltage	DIELECTRIC	Qty

""C002""	""CAP123""	""D15XB60H 15A 600V BRIDGE DIODE SINGLE IN LINE PACKAGE""	""D15XBXXHV""	""""	""""	""""	""TH""	""""	""""	""""	""""
""PCB1""	""PCB1012""	""Klimax 800W Mono Amp""	""""	""""	""""	""""	""""	""""	""""	""""";

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
            this.Board.Components.Should().HaveCount(1);
        }

        [Test]
        public void ShouldMakeSuggestedChangesMessage()
        {
            this.result.Message.Should().Contain("THE FOLLOWING CHANGES HAVE BEEN MADE FOR BOARD 123 revision L1R1");
            this.result.Message.Should().Contain("No changes found in selected file.");
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.result.Success.Should().BeTrue();
        }
    }
}
