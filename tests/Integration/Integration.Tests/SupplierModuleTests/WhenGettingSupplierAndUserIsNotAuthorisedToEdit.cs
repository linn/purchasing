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

    public class WhenGettingSupplierAndUserIsNotAuthorisedToEdit : ContextBase
    {
        private int id;

        private Supplier supplier;

        [SetUp]
        public void SetUp()
        {
            this.id = 1;
            this.supplier = new Supplier
                                {
                                    SupplierId = 1, 
                                    Name = "SUPPLIER", 
                                    OpenedBy = new Employee { Id = 1 }
                                };

            this.MockSupplierRepository.FindById(1).Returns(this.supplier);
            this.MockAuthService.HasPermissionFor(AuthorisedAction.SupplierUpdate, Arg.Any<IEnumerable<string>>())
                .Returns(false);

            this.Response = this.Client.Get(
                $"/purchasing/suppliers/{this.id}",
                with =>
                    {
                        with.Accept("application/json");
                    }).Result;
        }

        [Test]
        public void ShouldNotBuildEditLink()
        {
            var resource = this.Response.DeserializeBody<SupplierResource>();
            resource.Links.Any(x => x.Rel == "edit").Should().BeFalse();
        }
    }
}
