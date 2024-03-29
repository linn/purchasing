﻿namespace Linn.Purchasing.Integration.Tests.SupplierModuleTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingSupplierAndUserIsAuthorisedToChangeHoldStatus : ContextBase
    {
        private int id;

        private Supplier supplier;

        [SetUp]
        public void SetUp()
        {
            this.id = 1;
            this.supplier = new Supplier { SupplierId = 1, Name = "SUPPLIER", OpenedBy = new Employee() };

            this.MockSupplierRepository.FindById(1).Returns(this.supplier);
            this.MockAuthService.HasPermissionFor(AuthorisedAction.SupplierHoldChange, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.Response = this.Client.Get(
                $"/purchasing/suppliers/{this.id}",
                with =>
                    {
                        with.Accept("application/json");
                    }).Result;
        }

        [Test]
        public void ShouldBuildHoldLink()
        {
            var resource = this.Response.DeserializeBody<SupplierResource>();
            resource.Links.Single(x => x.Rel == "hold").Href.Should()
                .Be($"/purchasing/suppliers/hold");
        }
    }
}
