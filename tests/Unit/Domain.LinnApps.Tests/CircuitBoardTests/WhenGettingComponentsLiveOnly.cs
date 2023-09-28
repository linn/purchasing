namespace Linn.Purchasing.Domain.LinnApps.Tests.CircuitBoardTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NUnit.Framework;

    public class WhenGettingComponentsLiveOnly : ContextBase
    {
        private IList<BoardComponent> results;

        [SetUp]
        public void SetUp()
        {
            this.Sut.Components = new List<BoardComponent>
                                      {
                                          new BoardComponent
                                              {
                                                  BoardCode = this.BoardCode,
                                                  BoardLine = 1,
                                                  CRef = "C001",
                                                  PartNumber = "CAP 123",
                                                  AssemblyTechnology = "SM",
                                                  ChangeState = "LIVE",
                                                  FromLayoutVersion = 1,
                                                  FromRevisionVersion = 1,
                                                  ToLayoutVersion = 1,
                                                  ToRevisionVersion = null,
                                                  AddChangeId = 8763458,
                                                  DeleteChangeId = null,
                                                  Quantity = 1
                                              },
                                          new BoardComponent
                                              {
                                                  BoardCode = this.BoardCode,
                                                  BoardLine = 2,
                                                  CRef = "C002",
                                                  PartNumber = "CAP 123",
                                                  AssemblyTechnology = "SM",
                                                  ChangeState = "PROPOS",
                                                  FromLayoutVersion = 1,
                                                  FromRevisionVersion = 1,
                                                  ToLayoutVersion = 1,
                                                  ToRevisionVersion = 2,
                                                  AddChangeId = 8763458,
                                                  DeleteChangeId = null,
                                                  Quantity = 1
                                              },
                                          new BoardComponent
                                              {
                                                  BoardCode = this.BoardCode,
                                                  BoardLine = 3,
                                                  CRef = "C003",
                                                  PartNumber = "CAP 123",
                                                  AssemblyTechnology = "SM",
                                                  ChangeState = "LIVE",
                                                  FromLayoutVersion = 2,
                                                  FromRevisionVersion = 1,
                                                  ToLayoutVersion = 2,
                                                  ToRevisionVersion = 2,
                                                  AddChangeId = 8763458,
                                                  DeleteChangeId = null,
                                                  Quantity = 1
                                              },
                                          new BoardComponent
                                              {
                                                  BoardCode = this.BoardCode,
                                                  BoardLine = 4,
                                                  CRef = "C004",
                                                  PartNumber = "CAP 123",
                                                  AssemblyTechnology = "SM",
                                                  ChangeState = "PROPOS",
                                                  FromLayoutVersion = 1,
                                                  FromRevisionVersion = 1,
                                                  ToLayoutVersion = null,
                                                  ToRevisionVersion = null,
                                                  AddChangeId = 8763458,
                                                  DeleteChangeId = null,
                                                  Quantity = 1
                                              },
                                          new BoardComponent
                                              {
                                                  BoardCode = this.BoardCode,
                                                  BoardLine = 5,
                                                  CRef = "C005",
                                                  PartNumber = "CAP 123",
                                                  AssemblyTechnology = "SM",
                                                  ChangeState = "PROPOS",
                                                  FromLayoutVersion = 1,
                                                  FromRevisionVersion = 1,
                                                  ToLayoutVersion = 2,
                                                  ToRevisionVersion = 3,
                                                  AddChangeId = 8763458,
                                                  DeleteChangeId = null,
                                                  Quantity = 1
                                              },
                                          new BoardComponent
                                              {
                                                  BoardCode = this.BoardCode,
                                                  BoardLine = 6,
                                                  CRef = "C006",
                                                  PartNumber = "CAP 123",
                                                  AssemblyTechnology = "SM",
                                                  ChangeState = "PROPOS",
                                                  FromLayoutVersion = 2,
                                                  FromRevisionVersion = 1,
                                                  ToLayoutVersion = 2,
                                                  ToRevisionVersion = 1,
                                                  AddChangeId = 8763458,
                                                  DeleteChangeId = null,
                                                  Quantity = 1
                                              },
                                          new BoardComponent
                                              {
                                                  BoardCode = this.BoardCode,
                                                  BoardLine = 7,
                                                  CRef = "C007",
                                                  PartNumber = "CAP 123",
                                                  AssemblyTechnology = "SM",
                                                  ChangeState = "PROPOS",
                                                  FromLayoutVersion = 1,
                                                  FromRevisionVersion = 1,
                                                  ToLayoutVersion = 2,
                                                  ToRevisionVersion = null,
                                                  AddChangeId = 8763458,
                                                  DeleteChangeId = null,
                                                  Quantity = 1
                                              },
                                          new BoardComponent
                                              {
                                                  BoardCode = this.BoardCode,
                                                  BoardLine = 8,
                                                  CRef = "C008",
                                                  PartNumber = "CAP 123",
                                                  AssemblyTechnology = "SM",
                                                  ChangeState = "PROPOS",
                                                  FromLayoutVersion = 2,
                                                  FromRevisionVersion = 1,
                                                  ToLayoutVersion = 3,
                                                  ToRevisionVersion = null,
                                                  AddChangeId = 8763458,
                                                  DeleteChangeId = null,
                                                  Quantity = 1
                                              },
                                          new BoardComponent
                                              {
                                                  BoardCode = this.BoardCode,
                                                  BoardLine = 9,
                                                  CRef = "C009",
                                                  PartNumber = "CAP 123",
                                                  AssemblyTechnology = "SM",
                                                  ChangeState = "PROPOS",
                                                  FromLayoutVersion = 2,
                                                  FromRevisionVersion = 1,
                                                  ToLayoutVersion = 3,
                                                  ToRevisionVersion = 1,
                                                  AddChangeId = 8763458,
                                                  DeleteChangeId = null,
                                                  Quantity = 1
                                              },
                                          new BoardComponent
                                              {
                                                  BoardCode = this.BoardCode,
                                                  BoardLine = 10,
                                                  CRef = "C010",
                                                  PartNumber = "CAP 123",
                                                  AssemblyTechnology = "SM",
                                                  ChangeState = "LIVE",
                                                  FromLayoutVersion = 2,
                                                  FromRevisionVersion = 2,
                                                  ToLayoutVersion = null,
                                                  ToRevisionVersion = null,
                                                  AddChangeId = 8763458,
                                                  DeleteChangeId = null,
                                                  Quantity = 1
                                              },
                                          new BoardComponent
                                              {
                                                  BoardCode = this.BoardCode,
                                                  BoardLine = 11,
                                                  CRef = "C011",
                                                  PartNumber = "CAP 123",
                                                  AssemblyTechnology = "SM",
                                                  ChangeState = "PROPOS",
                                                  FromLayoutVersion = 2,
                                                  FromRevisionVersion = 3,
                                                  ToLayoutVersion = null,
                                                  ToRevisionVersion = null,
                                                  AddChangeId = 8763458,
                                                  DeleteChangeId = null,
                                                  Quantity = 1
                                              },
                                          new BoardComponent
                                              {
                                                  BoardCode = this.BoardCode,
                                                  BoardLine = 12,
                                                  CRef = "C011",
                                                  PartNumber = "CAP 123",
                                                  AssemblyTechnology = "SM",
                                                  ChangeState = "PROPOS",
                                                  FromLayoutVersion = 3,
                                                  FromRevisionVersion = 1,
                                                  ToLayoutVersion = null,
                                                  ToRevisionVersion = null,
                                                  AddChangeId = 8763458,
                                                  DeleteChangeId = null,
                                                  Quantity = 1
                                              },
                                          new BoardComponent
                                              {
                                                  BoardCode = this.BoardCode,
                                                  BoardLine = 13,
                                                  CRef = "C010",
                                                  PartNumber = "CAP 123",
                                                  AssemblyTechnology = "SM",
                                                  ChangeState = "HIST",
                                                  FromLayoutVersion = 2,
                                                  FromRevisionVersion = 2,
                                                  ToLayoutVersion = null,
                                                  ToRevisionVersion = null,
                                                  AddChangeId = 8763458,
                                                  DeleteChangeId = null,
                                                  Quantity = 1
                                              },
                                      };
            this.results = this.Sut.ComponentsOnRevision(2, 2, true);
        }

        [Test]
        public void ShouldGetComponent()
        {
            this.results.Should().HaveCount(2);
            this.results.Select(a => a.BoardLine).Should().Contain(3);
            this.results.Select(a => a.BoardLine).Should().NotContain(4);
            this.results.Select(a => a.BoardLine).Should().NotContain(5);
            this.results.Select(a => a.BoardLine).Should().NotContain(7);
            this.results.Select(a => a.BoardLine).Should().Contain(10);
        }
    }
}
