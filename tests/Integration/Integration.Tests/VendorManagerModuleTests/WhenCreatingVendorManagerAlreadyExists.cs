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

    public class WhenCreatingVendorManagerAlreadyExists : ContextBase
    {
        private string id;

        private VendorManagerResource vendorManager;

        private VendorManager currentVendorManagers;
        

        [SetUp]
        public void SetUp()
        {
            this.id = "M";
            this.vendorManager = new VendorManagerResource { VmId = this.id, Name = "vm1", PmMeasured = "Y", UserNumber = 10 };
            this.currentVendorManagers = new VendorManager { Id = this.id, PmMeasured = "Y", UserNumber = 10 };

            this.VendorManagerRepository.FindBy(Arg.Any<Expression<Func<VendorManager, bool>>>()).Returns(this.currentVendorManagers);

            this.Response = this.Client.PostAsJsonAsync("/purchasing/vendor-managers", this.vendorManager).Result;
        }

        [Test]
        public void ShouldReturnBadRequest()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
