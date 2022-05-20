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
        public void ShouldReturnOptions()
        {
            this.result.DangerLevelOptions.Should().HaveCount(1);
            this.result.PartSelectorOptions.Should().HaveCount(3);
            this.result.PartSelectorOptions.First(a => a.DisplaySequence == 0).Option.Should().Be("Select Parts");
            this.result.PartSelectorOptions.First(a => a.DisplaySequence == 1).Option.Should().Be("Planner3");
            this.result.PartSelectorOptions.First(a => a.DisplaySequence == 2).Option.Should().Be("Planner1");
        }
    }
}
