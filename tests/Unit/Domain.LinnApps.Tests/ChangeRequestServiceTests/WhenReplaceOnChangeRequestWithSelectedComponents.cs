namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenReplaceOnChangeRequestWithSelectedComponents : ContextBase
    {
        private ChangeRequest result;

        [SetUp]
        public void SetUp()
        {
            var request = new ChangeRequest
                              {
                                  DocumentNumber = 1,
                                  ChangeState = "ACCEPT",
                                  DateEntered = new DateTime(2022, 1, 1),
                                  DescriptionOfChange = "Test Change",
                                  BomChanges = new List<BomChange>(),
                                  OldPartNumber = "OLD 001",
                                  NewPartNumber = "NEW 002"
                              };
            this.Repository.FindById(1).Returns(request);

            var board = new CircuitBoard
                            {
                                BoardCode = "001",
                                Description = "Very Board",
                                Layouts = new List<BoardLayout>
                                              {
                                                  new BoardLayout
                                                      {
                                                          LayoutCode = "L1",
                                                          LayoutNumber = 1,
                                                          LayoutSequence = 1,
                                                          Revisions = new List<BoardRevision>
                                                                          {
                                                                              new BoardRevision
                                                                                  {
                                                                                      RevisionCode = "L1R1",
                                                                                      VersionNumber = 1,
                                                                                      LayoutSequence = 1
                                                                                  }
                                                                          }
                                                      }
                                              }
                            };
            this.BoardRepository.FindById("001").Returns(board);

            this.AuthService
                .HasPermissionFor(AuthorisedAction.AdminChangeRequest, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            var detailIds = new List<int> {};
            var addToBoms = new List<(string bomName, decimal qty)> { ("TEST", 1) };
            var selectedComponents = new List<string> { "001/L1R1/C001/1/TH" };
            this.result = this.Sut.Replace(1, 7004, false, false, null, detailIds, selectedComponents, addToBoms, new List<string>());
        }

        [Test]
        public void ShouldReturnChangeRequest()
        {
            this.result.Should().NotBeNull();
            this.result.ChangeState.Should().Be("ACCEPT");
        }

        [Test]
        public void ShouldNotCallReplaceonSelectedDetailIds()
        {
            this.BomChangeService.DidNotReceive().ReplaceBomDetail(Arg.Any<int>(), Arg.Any<ChangeRequest>(), 7004, null);
        }

        [Test]
        public void ShouldLookupBoard()
        {
            this.BoardRepository.Received().FindById("001");
        }

        [Test]
        public void ShouldCallPcasPack()
        {
            this.PcasPack.Received().ReplacePartWith("001", "L1R1", 1, 1, 1, "ACCEPT", 7004, "C001", "OLD 001", "NEW 002", 1, "TH");
        }

    }
}
