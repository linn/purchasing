namespace Linn.Purchasing.Integration.Tests.VendorManagerModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreatingVendorManager : ContextBase
    {
        private string id;

        private VendorManagerResource vendorManager;

        private List<VendorManager> vendorManagers;

        private Employee employee;

        [SetUp]
        public void SetUp()
        {
            this.id = "M";
            this.vendorManager = new VendorManagerResource { VmId = this.id, Name = "vm1", PmMeasured = "Y", UserNumber = 10 };
            this.employee = new Employee { Id = 10, FullName = "vm1" };

            this.vendorManagers = new List<VendorManager>
                                      {
                                          new VendorManager
                                              {
                                                  Id = "A",
                                                  PmMeasured = "Y",
                                                  UserNumber = 20,
                                                  Employee = new Employee { FullName = "vm2", Id = 20 }
                                              },
                                          new VendorManager
                                              {
                                                  Id = "B",
                                                  PmMeasured = "Y",
                                                  UserNumber = 30,
                                                  Employee = new Employee { FullName = "vm3", Id = 30 }
                                              }
                                      };

            this.VendorManagerRepository.FindAll()
                .Returns(this.vendorManagers.AsQueryable());

            this.Response = this.Client.PostAsJsonAsync("/purchasing/vendor-managers", this.vendorManager).Result;
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Test]
        public void ShouldReturnProcessResultWithSuccess()
        {
            var result = this.Response.DeserializeBody<VendorManagerResource>();
            result.Name.Should().Contain("vm1");
            result.PmMeasured.Should().Contain("Y");
            result.UserNumber.Should().Be(10);
        }
    }
}
