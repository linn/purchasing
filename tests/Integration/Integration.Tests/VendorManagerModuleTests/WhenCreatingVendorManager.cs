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

        [SetUp]
        public void SetUp()
        {
            this.id = "M";
            this.vendorManager = new VendorManagerResource { VmId = this.id, Name = "vm1", PmMeasured = "Y", UserNumber = 10 };

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
            result.PmMeasured.Should().Contain("Y");
            result.UserNumber.Should().Be(10);
        }
    }
}
