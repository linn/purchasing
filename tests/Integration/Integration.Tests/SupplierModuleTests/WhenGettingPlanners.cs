namespace Linn.Purchasing.Integration.Tests.SupplierModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingPlanners : ContextBase
    {
        private List<Planner> dataResult;

        [SetUp]
        public void SetUp()
        {
            this.dataResult = new List<Planner>
                                  {
                                      new Planner
                                          {
                                              Id = 1
                                          }
                                  };

            this.MockEmployeeRepository.FindById(1).Returns(new Employee { FullName = "MC PLANNER" });
            this.MockPlannerRepository.FindAll().Returns(this.dataResult.AsQueryable());
            this.Response = this.Client.Get(
                $"/purchasing/suppliers/planners",
                with =>
                    {
                        with.Accept("application/json");
                    }).Result;
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldReturnJsonContentType()
        {
            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resources = this.Response.DeserializeBody<IEnumerable<PlannerResource>>()?.ToArray();
            resources.Should().NotBeNull();
            resources.Should().HaveCount(1);
        }

        [Test]
        public void ShouldBuildResource()
        {
            var resources = this.Response.DeserializeBody<IEnumerable<PlannerResource>>().ToArray();
            resources.First().Id.Should().Be(1);
            resources.First().EmployeeName.Should().Be("MC PLANNER");
        }
    }
}
