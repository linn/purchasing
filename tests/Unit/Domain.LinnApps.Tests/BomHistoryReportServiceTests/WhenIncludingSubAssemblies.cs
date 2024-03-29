﻿namespace Linn.Purchasing.Domain.LinnApps.Tests.BomHistoryReportServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenIncludingSubAssemblies : ContextBase
    {
        private IEnumerable<BomHistoryReportLine> result;

        [SetUp]
        public void SetUp()
        {
            var detailRepoFindAllReturn = new List<BomDetail>();
            this.BomRepository
                .FindBy(Arg.Any<Expression<Func<Bom, bool>>>()).Returns(
                    new Bom { BomId = 40149 },
                    new Bom { BomId = 40121 },
                    new Bom { BomId = 40393 },
                    new Bom { BomId = 40394 });
            var rootSubAssemblies = new List<BomDetail>
                           {
                               new BomDetail
                                   {
                                       BomId = 40149, 
                                       PartNumber = "PCAS 1008/L2R2",
                                       ChangeState = "LIVE",
                                       AddChange = new BomChange { DateApplied = 11.November(2022) },
                                       Part = new Part { BomId = 41004 }
                                   },
                               new BomDetail
                                   {
                                       BomId = 40149, 
                                       PartNumber = "SELEKT DIAL/10",
                                       ChangeState = "HIST",
                                       AddChange = new BomChange { DateApplied = 02.September(2021) },
                                       DeleteChange = new BomChange { DateApplied = 22.June(2022) },
                                       Part = new Part { BomId = 40121 }
                                   },
                               new BomDetail
                                   {
                                       BomId = 40149, 
                                       PartNumber = "PCAS 1008/L2R2",
                                       ChangeState = "HIST",
                                       AddChange = new BomChange { DateApplied = 5.May(2022) },
                                       DeleteChange = new BomChange { DateApplied = 7.May(2022) },
                                       Part = new Part { BomId = 1008 }
                                   },
                               new BomDetail
                                   {
                                       BomId = 40149, 
                                       PartNumber = "MCAS 073",
                                       ChangeState = "LIVE",
                                       AddChange = new BomChange { DateApplied = 7.October(2022) },
                                       Part = new Part { BomId = 40393 }
                                   },
                               new BomDetail
                                   {
                                       BomId = 40149,
                                       PartNumber = "MCAS 074",
                                       ChangeState = "HIST",
                                       AddChange = new BomChange { DateApplied = 5.May(2022) },
                                       DeleteChange = new BomChange { DateApplied = 28.February(2023) },
                                       Part = new Part { BomId = 40394 } 
                                   }
                           };

            var selektDialDetails = new List<BomDetail>
                                              {
                                                  new BomDetail
                                                        {
                                                            BomId = 40121,
                                                            PartNumber = "SOME COMP",
                                                            ChangeState = "LIVE",
                                                            Part = new Part(),
                                                            AddChange = new BomChange { DateApplied = 7.October(2019) }
                                                        },
                                                    new BomDetail
                                                        {
                                                            BomId = 40121,
                                                            PartNumber = "LBL 111",
                                                            ChangeState = "LIVE",
                                                            Part = new Part(),
                                                            AddChange = new BomChange { DateApplied = 7.October(2022) }
                                                        },
                                                    new BomDetail
                                                        {
                                                            BomId = 40121,
                                                            PartNumber = "LBL 112",
                                                            ChangeState = "LIVE",
                                                            Part = new Part(),
                                                            AddChange = new BomChange { DateApplied = 28.February(2022) }
                                                        }
                                              };

            var mcas073Details = new List<BomDetail>
                                  {
                                        new BomDetail
                                            {
                                                BomId = 40393,
                                                PartNumber = "THING A",
                                                ChangeState = "LIVE",
                                                Part = new Part(),
                                                AddChange = new BomChange { DateApplied = 8.October(2022) }
                                            },
                                        new BomDetail 
                                            {
                                                BomId = 40393,
                                                PartNumber = "THING B",
                                                ChangeState = "LIVE",
                                                Part = new Part(),
                                                AddChange = new BomChange { DateApplied = 8.October(2022) }
                                            },
                                        new BomDetail
                                            {
                                                BomId = 40393,
                                                PartNumber = "THING C",
                                                ChangeState = "HIST",
                                                Part = new Part(),
                                                AddChange = new BomChange { DateApplied = 8.October(2018) },
                                                DeleteChange = new BomChange { DateApplied = 8.October(2022) },
                                            }
                                  };

            var mcas074Details = new List<BomDetail>
                                     {
                                         new BomDetail
                                             {
                                                 BomId = 40393,
                                                 PartNumber = "MCP 100",
                                                 ChangeState = "LIVE",
                                                 Part = new Part(),
                                                 AddChange = new BomChange { DateApplied = 8.October(2022) }
                                             }
                                     };

            var pcasDetails = new List<BomDetail>
                                  {
                                      new BomDetail
                                          {
                                              BomId = 1008,
                                              PartNumber = "PCSM 100",
                                              ChangeState = "LIVE",
                                              Part = new Part { BomId = 600 },
                                              AddChange = new BomChange { DateApplied = 4.May(2022) }
                                          }
                                  };

            var pcsmDetails = new List<BomDetail>
                                  {
                                      new BomDetail 
                                          {
                                              BomId = 600, 
                                              PartNumber = "CAP 100", 
                                              ChangeState = "LIVE",
                                              Part = new Part(),
                                              AddChange = new BomChange { DateApplied = 5.May(2022) }
                                          }
                                  };
            detailRepoFindAllReturn.AddRange(rootSubAssemblies);
            detailRepoFindAllReturn.AddRange(selektDialDetails);
            detailRepoFindAllReturn.AddRange(mcas073Details);
            detailRepoFindAllReturn.AddRange(mcas074Details);
            detailRepoFindAllReturn.AddRange(pcasDetails);
            detailRepoFindAllReturn.AddRange(pcsmDetails);

            var historyEntries = new List<BomHistoryViewEntry>
                                     {
                                         new BomHistoryViewEntry
                                             {
                                                 BomName = "SK HUB",
                                                 ChangeId = 1,
                                                 DateApplied = 28.March(1995),
                                                 Operation = "added",
                                                 PartNumber = "CAP 530"
                                             },
                                         new BomHistoryViewEntry
                                             {
                                                 BomName = "SK HUB",
                                                 ChangeId = 2,
                                                 DateApplied = 11.November(2022),
                                                 Operation = "added",
                                                 PartNumber = "PCAS 1008/L2R2"
                                             },
                                         new BomHistoryViewEntry
                                             {
                                                 BomName = "SK HUB",
                                                 ChangeId = 3,
                                                 DateApplied = 02.September(2021),
                                                 Operation = "added",
                                                 PartNumber = "SELEKT DIAL/10"
                                             },
                                         new BomHistoryViewEntry
                                             {
                                                 BomName = "SK HUB",
                                                 ChangeId = 4,
                                                 DateApplied = 22.June(2022),
                                                 Operation = "deleted",
                                                 PartNumber = "SELEKT DIAL/10"
                                             },
                                         new BomHistoryViewEntry
                                             {
                                                 BomName = "SK HUB",
                                                 ChangeId = 5,
                                                 DateApplied = 5.May(2022),
                                                 Operation = "added",
                                                 PartNumber = "PCAS 1008/L2R2"
                                             },
                                         new BomHistoryViewEntry
                                             {
                                                 BomName = "SK HUB",
                                                 ChangeId = 6,
                                                 DateApplied = 7.May(2022),
                                                 Operation = "deleted",
                                                 PartNumber = "PCAS 1008/L2R2"
                                             },
                                         new BomHistoryViewEntry
                                             {
                                                 BomName = "SK HUB",
                                                 ChangeId = 7,
                                                 DateApplied = 5.May(2022),
                                                 Operation = "added",
                                                 PartNumber = "MCAS 073"
                                             },
                                         new BomHistoryViewEntry
                                             {
                                                 BomName = "SK HUB",
                                                 ChangeId = 98,
                                                 DateApplied = 5.May(2022),
                                                 Operation = "added",
                                                 PartNumber = "MCAS 074"
                                             },
                                         new BomHistoryViewEntry
                                             {
                                                 BomName = "SK HUB",
                                                 ChangeId = 99,
                                                 DateApplied = 28.February(2023),
                                                 Operation = "deleted",
                                                 PartNumber = "MCAS 074"
                                             },
                                         new BomHistoryViewEntry
                                             {
                                                 BomName = "SELEKT DIAL/10",
                                                 ChangeId = 8,
                                                 DateApplied = 1.September(2020),
                                                 Operation = "added",
                                                 PartNumber = "SOME COMP1"
                                             },
                                         new BomHistoryViewEntry
                                             {
                                                 BomName = "SELEKT DIAL/10",
                                                 ChangeId = 8,
                                                 DateApplied = 23.June(2022),
                                                 Operation = "added",
                                                 PartNumber = "SOME COMP2"
                                             },
                                         new BomHistoryViewEntry
                                             {
                                                 BomName = "SELEKT DIAL/10",
                                                 ChangeId = 9,
                                                 DateApplied = 21.June(2022),
                                                 Operation = "added",
                                                 PartNumber = "LBL 111"
                                             },
                                         new BomHistoryViewEntry
                                             {
                                                 BomName = "SELEKT DIAL/10",
                                                 ChangeId = 12,
                                                 DateApplied = 28.February(2023),
                                                 Operation = "added",
                                                 PartNumber = "LBL 112"
                                             },
                                         new BomHistoryViewEntry
                                             {
                                                 BomName = "MCAS 073",
                                                 ChangeId = 10,
                                                 DateApplied = 8.October(2022),
                                                 Operation = "added",
                                                 PartNumber = "THING A"
                                             },
                                         new BomHistoryViewEntry
                                             {
                                                 BomName = "MCAS 073",
                                                 ChangeId = 10,
                                                 DateApplied = 8.October(2022),
                                                 Operation = "added",
                                                 PartNumber = "THING B"
                                             },
                                         new BomHistoryViewEntry
                                             {
                                                 BomName = "MCAS 073",
                                                 ChangeId = 10,
                                                 DateApplied = 8.October(2022),
                                                 Operation = "deleted",
                                                 PartNumber = "THING C"
                                             },
                                         new BomHistoryViewEntry
                                             {
                                                 BomName = "MCAS 074",
                                                 ChangeId = 1113,
                                                 DateApplied = 8.October(2022),
                                                 Operation = "deleted",
                                                 PartNumber = "MCP 100"
                                             },
                                         new BomHistoryViewEntry
                                             {
                                                 BomName = "PCAS 1008/L2R2",
                                                 ChangeId = 1000,
                                                 DateApplied = 4.May(2022),
                                                 Operation = "added",
                                                 PartNumber = "PCSM 100"
                                             },
                                         new BomHistoryViewEntry
                                             {
                                                 BomName = "PCAS 100",
                                                 ChangeId = 666,
                                                 DateApplied = 5.May(2022),
                                                 Operation = "added",
                                                 PartNumber = "CAP 100"
                                             }
                                     };

            this.BomHistoryViewEntryRepository.FindAll().Returns(historyEntries.AsQueryable());

            this.DetailRepository.FindAll().Returns(detailRepoFindAllReturn.AsQueryable());

            this.result = this.Sut.GetReportWithSubAssemblies("SK HUB", 28.February(2020), 27.February(2023));
        }

        [Test]
        public void ShouldOnlyIncludeChangesToRootThatOccurredWithinDateRange()
        {
            this.result.Count(x => x.BomName == "SK HUB").Should().Be(7);
        }

        [Test]
        public void ShouldIncludeChangesToSubAssembliesThatHappenedWhileTheyWereOnBom()
        {
            // since SOME COMP1 was added to SELEKT DIAL/10 before it was added to its parent
            this.result.Where(r => r.BomName == "SELEKT DIAL/10")
                .Any(x => x.PartNumber == "SOME COMP1").Should()
                .BeFalse();

            // since SOME COMP2 was added to SELEKT DIAL/10 after it was deleted from its parent
            this.result.Where(r => r.BomName == "SELEKT DIAL/10")
                .Any(x => x.PartNumber == "SOME COMP1").Should()
                .BeFalse();

            // since LBL 111 was added after SELEKT DIAL/10 was added to its parent, but before SELEKT DIAL/10 was deleted from its parent
            this.result.Where(r => r.BomName == "SELEKT DIAL/10")
                .Count(x => x.PartNumber == "LBL 111").Should().Be(1);

            // since MCAS 074 was deleted from bom outside the time range
            // any changes to it in the time range should be included
            this.result.Where(r => r.BomName == "MCAS 074")
                .Count(x => x.PartNumber == "MCP 100").Should().Be(1);

            // since PCAS was added to its parent after it had any changes to itself
            this.result.Any(r => r.BomName == "PCAS 1008/L2R2").Should()
                .BeFalse();
        }

        [Test]
        public void ShouldOnlyIncludeChangesToSubAssembliesThatHappenedWithinDateRange()
        {
            this.result.Where(r => r.BomName == "SELEKT DIAL/10")
                .Any(x => x.PartNumber == "LBL 112").Should()
                .BeFalse();
        }

        [Test]
        public void ShouldGroupChangeLinesByDetail()
        {
            this.result.Count(x => x.BomName == "MCAS 073").Should().Be(1);
            this.result.First(x => x.BomName == "MCAS 073")
                .Operation.Should().Be($"deleted{Environment.NewLine}added{Environment.NewLine}added");
            this.result.First(x => x.BomName == "MCAS 073")
                .PartNumber.Should().Be($"THING C{Environment.NewLine}THING A{Environment.NewLine}THING B");
        }
    }
}
