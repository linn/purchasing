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
                                         PmMeasured = "Y",
                                         UserNumber = 10,
                                         Employee = new Employee
                                                        {
                                                            FullName = "vm1",
                                                            Id = 10
                                                        }
                                     };

            this.vendorManagers = new List<VendorManager>
                                      {
                                          this.vendorManager,
                                          new VendorManager
                                              {
                                                  Id = "B",
                                                  PmMeasured = "Y",
                                                  UserNumber = 20,
                                                  Employee = new Employee { FullName = "vm2", Id = 20 }
                                              }
                                      };

            this.VendorManagerRepository.FindById(this.id)
                .Returns(this.vendorManager);

            this.VendorManagerRepository.FindBy(Arg.Any<Expression<Func<VendorManager, bool>>>())
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
        public void ShouldReturnBadRequest()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        
    }
}
