namespace Linn.Purchasing.Integration.Tests.VendorManagerModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingVendorManagerAlreadyExists : ContextBase
    {
        private string id;

        private VendorManagerResource vendorManager;

        private VendorManager currentVendorManagers;

        private List<VendorManager> vendorManagers;

        private VendorManagerResource updatedVendorManager;

        [SetUp]
        public void SetUp()
        {
            this.id = "M";
            this.vendorManager = new VendorManagerResource { VmId = this.id, Name = "vm1", PmMeasured = "Y", UserNumber = 10 };
            this.updatedVendorManager = new VendorManagerResource { VmId = this.id, Name = "vm2", PmMeasured = "Y", UserNumber = 20 };

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

            this.VendorManagerRepository.FindBy(Arg.Any<Expression<Func<VendorManager, bool>>>()).Returns(this.currentVendorManagers);

            this.Response = this.Client.PutAsJsonAsync($"/purchasing/vendor-managers/{this.id}", this.updatedVendorManager).Result;
        }

        [Test]
        public void ShouldReturnBadRequest()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
