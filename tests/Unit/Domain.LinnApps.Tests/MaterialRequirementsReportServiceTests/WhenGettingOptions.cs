namespace Linn.Purchasing.Domain.LinnApps.Tests.MaterialRequirementsReportServiceTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingOptions : ContextBase
    {
        private MrReportOptions result;

        [SetUp]
        public void SetUp()
        {
            this.PlannerRepository.FindAll()
                .Returns(new List<Planner>
                             {
                                 new Planner { Id = 1, ShowAsMrOption = "Y" },
                                 new Planner { Id = 2, ShowAsMrOption = "N" },
                                 new Planner { Id = 3, ShowAsMrOption = "Y" }
                             }.AsQueryable());
            this.EmployeeRepository.FindById(1).Returns(new Employee { FullName = "Yvonne" });
            this.EmployeeRepository.FindById(3).Returns(new Employee { FullName = "Alan" });
            this.result = this.Sut.GetOptions();
        }

        [Test]
        public void ShouldReturnPartSelectorOptions()
        {
            this.result.PartSelectorOptions.Should().HaveCount(9);
            this.result.PartSelectorOptions.First(a => a.DisplaySequence == 0).Option.Should().Be("Select Parts");
            this.result.PartSelectorOptions.First(a => a.DisplaySequence == 1).Option.Should().Be("Parts Used On");
            this.result.PartSelectorOptions.First(a => a.DisplaySequence == 2).Option.Should().Be("Assemblies Used On");
            this.result.PartSelectorOptions.First(a => a.DisplaySequence == 3).Option.Should().Be("Parts Where Used");
            this.result.PartSelectorOptions.First(a => a.DisplaySequence == 4).Option.Should().Be("Supplier");
            this.result.PartSelectorOptions.First(a => a.DisplaySequence == 5).Option.Should().Be("Part Number List");
            this.result.PartSelectorOptions.First(a => a.DisplaySequence == 6).Option.Should().Be("Stock Category Name");
            this.result.PartSelectorOptions.First(a => a.DisplaySequence == 7).Option.Should().Be("Planner3");
            this.result.PartSelectorOptions.First(a => a.DisplaySequence == 8).Option.Should().Be("Planner1");
        }

        [Test]
        public void ShouldReturnStockLevelOptions()
        {
            this.result.StockLevelOptions.Should().HaveCount(11);
            this.result.StockLevelOptions.First(a => a.DisplaySequence == 0).Option.Should().Be("0-4");
            this.result.StockLevelOptions.First(a => a.DisplaySequence == 0).DisplayText.Should().Be("Danger Levels 0 - 4");
            this.result.StockLevelOptions.First(a => a.DisplaySequence == 1).Option.Should().Be("0-2");
            this.result.StockLevelOptions.First(a => a.DisplaySequence == 1).DisplayText.Should().Be("Danger Levels 0 - 2"); 
            this.result.StockLevelOptions.First(a => a.DisplaySequence == 2).Option.Should().Be("All");
            this.result.StockLevelOptions.First(a => a.DisplaySequence == 2).DisplayText.Should().Be("All Stock Levels"); 
            this.result.StockLevelOptions.First(a => a.DisplaySequence == 3).Option.Should().Be("0");
            this.result.StockLevelOptions.First(a => a.DisplaySequence == 3).DisplayText.Should().Be("Danger Level 0 Short for triggered builds"); 
            this.result.StockLevelOptions.First(a => a.DisplaySequence == 4).Option.Should().Be("1");
            this.result.StockLevelOptions.First(a => a.DisplaySequence == 4).DisplayText.Should().Be("Danger Level 1 Short now"); 
            this.result.StockLevelOptions.First(a => a.DisplaySequence == 5).Option.Should().Be("2");
            this.result.StockLevelOptions.First(a => a.DisplaySequence == 5).DisplayText.Should().Be("Danger Level 2 Zero at lead time"); 
            this.result.StockLevelOptions.First(a => a.DisplaySequence == 6).Option.Should().Be("3");
            this.result.StockLevelOptions.First(a => a.DisplaySequence == 6).DisplayText.Should().Be("Danger Level 3 Low at lead time"); 
            this.result.StockLevelOptions.First(a => a.DisplaySequence == 7).Option.Should().Be("4");
            this.result.StockLevelOptions.First(a => a.DisplaySequence == 7).DisplayText.Should().Be("Danger Level 4 Very low before lead time");
            this.result.StockLevelOptions.First(a => a.DisplaySequence == 8).Option.Should().Be("Late");
            this.result.StockLevelOptions.First(a => a.DisplaySequence == 8).DisplayText.Should().Be("Late Orders");
            this.result.StockLevelOptions.First(a => a.DisplaySequence == 9).Option.Should().Be("High With Orders");
            this.result.StockLevelOptions.First(a => a.DisplaySequence == 9).DisplayText.Should().Be("High Stock With Orders");
            this.result.StockLevelOptions.First(a => a.DisplaySequence == 10).Option.Should().Be("High With No Orders");
            this.result.StockLevelOptions.First(a => a.DisplaySequence == 10).DisplayText.Should().Be("High Stock With No Orders");
        }

        [Test]
        public void ShouldReturnOrderByOptions()
        {
            this.result.OrderByOptions.Should().HaveCount(2);
            this.result.OrderByOptions.First(a => a.DisplaySequence == 0).Option.Should().Be("supplier/part");
            this.result.OrderByOptions.First(a => a.DisplaySequence == 1).Option.Should().Be("part");
        }

        [Test]
        public void ShouldReturnPartOptions()
        {
            this.result.PartOptions.Should().HaveCount(5);
            this.result.PartOptions.First(a => a.DisplaySequence == 0).Option.Should().Be("Long Lead Time");
            this.result.PartOptions.First(a => a.DisplaySequence == 1).Option.Should().Be("CAP");
            this.result.PartOptions.First(a => a.DisplaySequence == 2).Option.Should().Be("RES");
            this.result.PartOptions.First(a => a.DisplaySequence == 3).Option.Should().Be("TRAN");
            this.result.PartOptions.First(a => a.DisplaySequence == 4).Option.Should().Be("Unacknowledged");
        }
    }
}
