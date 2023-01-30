namespace Linn.Purchasing.Domain.LinnApps.Tests.CircuitBoardServiceTests
{
    using System.Collections.Generic;

    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected ICircuitBoardService Sut { get; private set; }
        
        protected IRepository<ChangeRequest, int> ChangeRequestRepository { get; private set; }
        
        protected IRepository<CircuitBoard, string> BoardRepository { get; private set; }
        
        protected IQueryRepository<Part> PartRepository { get; private set; }

        protected string BoardCode { get; set; }

        protected CircuitBoard Board { get; set; }

        protected int ChangeRequestId { get; set; }

        protected ChangeRequest ChangeRequest { get; set; }

        protected int ChangeId { get; set; }

        protected PcasChange PcasChange { get; set; }

        [SetUp]
        public void EstablishContext()
        {
            this.ChangeRequestRepository = Substitute.For<IRepository<ChangeRequest, int>>();
            this.BoardRepository = Substitute.For<IRepository<CircuitBoard, string>>();
            this.PartRepository = Substitute.For<IQueryRepository<Part>>();
            const string RevisionBeingChanged = "L1R1";

            this.ChangeRequestId = 678;
            this.ChangeRequest = new ChangeRequest
                                     {
                                         DocumentNumber = this.ChangeRequestId,
                                         BoardCode = this.BoardCode,
                                         RevisionCode = RevisionBeingChanged,
                                         ChangeState = "PROPOS"
                                     };
            this.ChangeRequestRepository.FindById(this.ChangeRequestId).Returns(this.ChangeRequest);

            this.ChangeId = 890;
            this.PcasChange = new PcasChange
                                  {
                                      BoardCode = this.BoardCode,
                                      ChangeId = this.ChangeId,
                                      ChangeRequest = this.ChangeRequest,
                                      ChangeState = "PROPOS",
                                      DocumentNumber = this.ChangeRequestId,
                                      RevisionCode = RevisionBeingChanged
                                  };

            this.BoardCode = "123";
            this.Board = new CircuitBoard
                             {
                                 BoardCode = this.BoardCode,
                                 Description = null,
                                 ChangeId = null,
                                 ChangeState = null,
                                 SplitBom = null,
                                 DefaultPcbNumber = null,
                                 VariantOfBoardCode = null,
                                 LoadDirectory = null,
                                 BoardsPerSheet = null,
                                 CoreBoard = null,
                                 ClusterBoard = null,
                                 IdBoard = null,
                                 Layouts = new List<BoardLayout>
                                               {
                                                   new BoardLayout
                                                       {
                                                           BoardCode = this.BoardCode,
                                                           LayoutCode = "L1",
                                                           LayoutNumber = 1,
                                                           LayoutSequence = 1,
                                                           LayoutType = "PRODUCTION",
                                                           Revisions = new List<BoardRevision>
                                                                           {
                                                                               new BoardRevision
                                                                                   {
                                                                                       BoardCode = this.BoardCode,
                                                                                       RevisionCode = "L1R1",
                                                                                       RevisionNumber = 1,
                                                                                       VersionNumber = 1,
                                                                                       LayoutCode = "L1",
                                                                                       LayoutSequence = 1
                                                                                   }
                                                                           }
                                                       }
                                               },
                                 Components = new List<BoardComponent>
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
                                                              ToLayoutVersion = null,
                                                              ToRevisionVersion = null,
                                                              AddChangeId = 8763458,
                                                              DeleteChangeId = null,
                                                              Quantity = 1
                                                          }
                                                  }
                             };
            
            this.BoardRepository.FindById(this.BoardCode).Returns(this.Board);

            this.Sut = new CircuitBoardService(this.ChangeRequestRepository, this.BoardRepository, this.PartRepository);
        }
    }
}
