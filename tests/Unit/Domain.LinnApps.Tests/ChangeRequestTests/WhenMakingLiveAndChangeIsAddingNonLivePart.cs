namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Boms.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    using NUnit.Framework;

    public class WhenMakingLiveAndChangeIsAddingNonLivePart : ContextBase
    {
        private Action action;
        [SetUp]
        public void SetUp()
        {
            this.Sut = new ChangeRequest
            {
                DocumentNumber = 1,
                ChangeState = "ACCEPT",
                BomChanges = new List<BomChange>
                                                {
                                                    new BomChange
                                                        {
                                                            ChangeId = 1, 
                                                            ChangeState = "ACCEPT", 
                                                            BomName = "ABOM",
                                                            AddedBomDetails = new List<BomDetail> 
                                                                                  { 
                                                                                      new BomDetail
                                                                                        {
                                                                                            PartNumber = "DETA",
                                                                                            Part = new Part { DateLive = null }
                                                                                        },
                                                                                      new BomDetail
                                                                                          {
                                                                                              PartNumber = "DETB",
                                                                                              Part = new Part { DateLive = null }
                                                                                          }
                                                                                  }
                                                        }
                                                }
            };

            var appliedBy = new Employee { Id = 1, FullName = "Stu Pot" };
            var liveBomIds = new List<int> { 1 };

             this.action = () => this.Sut.MakeLive(appliedBy, liveBomIds, null);
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<InvalidStateChangeException>().WithMessage("Cannot add Non live parts to bom: DETA, DETB");
        }

        [Test]
        public void ShouldStillBeInAcceptState()
        {
            this.Sut.ChangeState.Should().Be("ACCEPT");
        }
    }
}
