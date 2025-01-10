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

    public class WhenUpdatingVendorManager : ContextBase
    {
        private string id;

        private VendorManager vendorManager;

        private List<VendorManager> vendorManagers;

        private VendorManagerResource updatedVendorManager;

        [SetUp]
        public void SetUp()
        {
            this.id = "M";
            this.vendorManager = new VendorManager 
                                     { 
                                                           Id = this.id, 
                                                           PmMeasured = "Y", UserNumber = 10,
                                                           Employee = new Employee 
                                                                           {
                                                                                FullName = "vm1",
                                                                                Id = 10
                                                                            }
                                     };

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

            this.VendorManagerRepository.FindById(this.id)
                .Returns(this.vendorManager);

            this.updatedVendorManager = new VendorManagerResource
                                            {
                                                VmId = this.id,
                                                PmMeasured = "N",
                                                UserNumber = 20,
                                                Name = "vm2"
            };

            this.Response = this.Client.PutAsJsonAsync($"/purchasing/vendor-managers/{this.id}", this.updatedVendorManager).Result;
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldReturnProcessResultWithSuccess()
        {
            var result = this.Response.DeserializeBody<VendorManagerResource>();
            result.PmMeasured.Should().Be("N");
            result.UserNumber.Should().Be(20);

        }
    }
}
