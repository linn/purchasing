namespace Linn.Purchasing.Domain.LinnApps.Tests.CircuitBoardServiceTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NUnit.Framework;

    public class WhenReadingTabSeparatedUploadFile
    {
        private string file;

        private IEnumerable<BoardComponent> results;

        private string pcbPartNumber;

        private TabSeparatedReadStrategy Sut { get; set; }

        [SetUp]
        public void SetUp()
        {
            this.file = @"Designator	Part No	Part Description	Footprint	Tolerance	Negative Tolerance	Positive Tolerance	Technology	Value	Voltage	DIELECTRIC	Qty

""BR100""	""MISS266""	""D15XB60H 15A 600V BRIDGE DIODE SINGLE IN LINE PACKAGE""	""D15XBXXHV""	""""	""""	""""	""TH""	""""	""""	""""	""""
""C100""	""CAP574""	""""	""CAPPRS10-35x30""	""""	""""	""""	""""	""1500UF""	""""	""""	""""
""PCB1""	""PCB791/L5""	""PCB""	""""	""""	""""	""""	""""	""""	""""	""""	""""";
            this.Sut = new TabSeparatedReadStrategy();

            (this.results, this.pcbPartNumber) = this.Sut.ReadFile(this.file);
        }

        [Test]
        public void ShouldNotIncludePcbInComponents()
        {
            this.results.Should().NotContain(a => a.CRef == "PCB1");
        }

        [Test]
        public void ShouldReturnPcb()
        {
            this.pcbPartNumber.Should().Be("PCB 791/L5");
        }

        [Test]
        public void ShouldReturnComponents()
        {
            this.results.Should().HaveCount(2);
            this.results.First(a => a.CRef == "BR100").PartNumber.Should().Be("MISS 266");
            this.results.First(a => a.CRef == "C100").PartNumber.Should().Be("CAP 574");
        }
    }
}
