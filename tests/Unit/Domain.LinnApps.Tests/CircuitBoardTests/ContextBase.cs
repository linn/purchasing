namespace Linn.Purchasing.Domain.LinnApps.Tests.CircuitBoardTests
{
    using System.Collections.Generic;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NUnit.Framework;

    public class ContextBase
    {
        protected CircuitBoard Sut { get; set; }

        protected string BoardCode { get; set; }

        [SetUp]
        public void ContextBaseSetUp()
        {
            this.BoardCode = "123";
            this.Sut = new CircuitBoard
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
                                                                                 },
                                                                             new BoardRevision
                                                                                 {
                                                                                     BoardCode = this.BoardCode,
                                                                                     RevisionCode = "L1R2",
                                                                                     RevisionNumber = 2,
                                                                                     VersionNumber = 2,
                                                                                     LayoutCode = "L1",
                                                                                     LayoutSequence = 1
                                                                                 }
                                                                         }
                                                     },
                                                 new BoardLayout
                                                     {
                                                         BoardCode = this.BoardCode,
                                                         LayoutCode = "L2",
                                                         LayoutNumber = 2,
                                                         LayoutSequence = 2,
                                                         LayoutType = "PRODUCTION",
                                                         Revisions = new List<BoardRevision>
                                                                         {
                                                                             new BoardRevision
                                                                                 {
                                                                                     BoardCode = this.BoardCode,
                                                                                     RevisionCode = "L2R1",
                                                                                     RevisionNumber = 1,
                                                                                     VersionNumber = 1,
                                                                                     LayoutCode = "L2",
                                                                                     LayoutSequence = 2
                                                                                 },
                                                                             new BoardRevision
                                                                                 {
                                                                                     BoardCode = this.BoardCode,
                                                                                     RevisionCode = "L2R2",
                                                                                     RevisionNumber = 2,
                                                                                     VersionNumber = 2,
                                                                                     LayoutCode = "L2",
                                                                                     LayoutSequence = 2
                                                                                 },
                                                                             new BoardRevision
                                                                                 {
                                                                                     BoardCode = this.BoardCode,
                                                                                     RevisionCode = "L2R3",
                                                                                     RevisionNumber = 3,
                                                                                     VersionNumber = 3,
                                                                                     LayoutCode = "L2",
                                                                                     LayoutSequence = 2
                                                                                 }
                                                                         }
                                                     },
                                                 new BoardLayout
                                                     {
                                                         BoardCode = this.BoardCode,
                                                         LayoutCode = "L3",
                                                         LayoutNumber = 3,
                                                         LayoutSequence = 3,
                                                         LayoutType = "PRODUCTION",
                                                         Revisions = new List<BoardRevision>
                                                                         {
                                                                             new BoardRevision
                                                                                 {
                                                                                     BoardCode = this.BoardCode,
                                                                                     RevisionCode = "L3R1",
                                                                                     RevisionNumber = 1,
                                                                                     VersionNumber = 1,
                                                                                     LayoutCode = "L3",
                                                                                     LayoutSequence = 3
                                                                                 },
                                                                             new BoardRevision
                                                                                 {
                                                                                     BoardCode = this.BoardCode,
                                                                                     RevisionCode = "L3R2",
                                                                                     RevisionNumber = 2,
                                                                                     VersionNumber = 2,
                                                                                     LayoutCode = "L3",
                                                                                     LayoutSequence = 3
                                                                                 }
                                                                         }
                                                     }
                                             },
                               Components = new List<BoardComponent>()
            };
        }
    }
}
