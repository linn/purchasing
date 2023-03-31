namespace Linn.Purchasing.Domain.LinnApps.Tests.CircuitBoardServiceTests
{
    using System;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCheckingSmtFile : ContextBase
    {
        private ProcessResult result;

        private string revision;

        private string file;

        [SetUp]
        public void SetUp()
        {
            this.revision = "L1R1";
            this.Board.Components.Add(new BoardComponent
                                          {
                                              BoardCode = this.BoardCode,
                                              BoardLine = 1,
                                              CRef = "C005",
                                              PartNumber = "CAP 789",
                                              AssemblyTechnology = "SM",
                                              ChangeState = "PROPOS",
                                              FromLayoutVersion = 1,
                                              FromRevisionVersion = 1,
                                              ToLayoutVersion = null,
                                              ToRevisionVersion = null,
                                              AddChangeId = 8763458,
                                              DeleteChangeId = null,
                                              Quantity = 1
                                          });
            this.Board.Components.Add(new BoardComponent
                                          {
                                              BoardCode = this.BoardCode,
                                              BoardLine = 1,
                                              CRef = "C006",
                                              PartNumber = "EDF 789",
                                              AssemblyTechnology = "TH",
                                              ChangeState = "PROPOS",
                                              FromLayoutVersion = 1,
                                              FromRevisionVersion = 1,
                                              ToLayoutVersion = null,
                                              ToRevisionVersion = null,
                                              AddChangeId = 8763458,
                                              DeleteChangeId = null,
                                              Quantity = 1
                                          });
            this.file = @"

  #C CAP011                          ; C5    ; 0.000; 129.604; 62.920
  #C CAP185                          ; C602    ; 90.000; 206.439; 11.231
  #C CAP214                          ; C130    ; 90.000; 70.295; 38.155";

            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>())
                .Returns(new Part());

            this.result = this.Sut.UpdateFromFile(
                this.BoardCode,
                this.revision,
                "SMT",
                this.file,
                null,
                false);
        }

        [Test]
        public void ShouldMakeSuggestedChangesMessage()
        {
            this.result.Message.Should().Contain("Differences found in file against board 123 revision L1R1");
            this.result.Message.Should().NotContain("Pcb part number on revision is  but found  in the file.");
            this.result.Message.Should().Contain("C602                - PC File = CAP 185         Database = _______________");
            this.result.Message.Should().Contain("C130                - PC File = CAP 214         Database = _______________");
            this.result.Message.Should().Contain("C002                - PC File = _______________ Database = CAP 123");
            this.result.Message.Should().NotContain("EDF 789");
            this.result.Message.Should().Contain("C005                - PC File = CAP 011         Database = CAP 789");
            this.result.Message.Should().NotContain("ERROR");
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.result.Success.Should().BeTrue();
        }
    }
}
