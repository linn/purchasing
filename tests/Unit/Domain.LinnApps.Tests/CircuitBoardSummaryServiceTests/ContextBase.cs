namespace Linn.Purchasing.Domain.LinnApps.Tests.CircuitBoardSummaryServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    using NUnit.Framework;

    public class ContextBase
    {
        protected ICircuitBoardSummaryService Sut { get; private set; }
        
        protected IQueryable<BoardComponentSummary> Components { get; set; }

        protected Expression<Func<BoardComponentSummary, bool>> Expression { get; set; }

        protected IEnumerable<BoardComponentSummary> Results { get; set; }

        [SetUp]
        public void EstablishContext()
        {
            this.Sut = new CircuitBoardSummaryService();
            this.Components = new List<BoardComponentSummary>
                                  {
                                      new BoardComponentSummary
                                          {
                                              BoardLine  = 808, BoardCode = "123", RevisionCode = "L1R1", Cref = "C001", PartNumber = "P1"
                                          },
                                      new BoardComponentSummary
                                          {
                                              BoardCode = "123", RevisionCode = "L1R2", Cref = "D001", PartNumber = "P2"
                                          },
                                      new BoardComponentSummary
                                          {
                                              BoardCode = "123", RevisionCode = "L2R1", Cref = "R456", PartNumber = "P3"
                                          },
                                      new BoardComponentSummary
                                          {
                                              BoardCode = "456", RevisionCode = "L1R1", Cref = "C011", PartNumber = "P1"
                                          }
                                  }.AsQueryable();
        }
    }
}
