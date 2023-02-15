namespace Linn.Purchasing.Domain.LinnApps.Tests.CircuitBoardTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NUnit.Framework;

    public class WhenGettingComponentsAtToValues : ContextBase
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
                                                  CRef = "C002",
                                                  PartNumber = "CAP 123",
                                                  AssemblyTechnology = "SM",
                                                  ChangeState = "PROPOS",
                                                  FromLayoutVersion = 1,
                                                  FromRevisionVersion = 1,
                                                  ToLayoutVersion = 2,
                                                  ToRevisionVersion = 1,
                                                  AddChangeId = 8763458,
                                                  DeleteChangeId = null,
                                                  Quantity = 1
                                              }
                                      };
            this.results = this.Sut.ComponentsOnRevision(2, 1);
        }

        [Test]
        public void ShouldGetComponent()
        {
            this.results.Should().HaveCount(1);
            this.results.First().BoardLine.Should().Be(1);
        }
    }
}
