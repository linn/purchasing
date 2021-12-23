﻿namespace Linn.Purchasing.Domain.LinnApps.Tests.PartSupplierServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreating : ContextBase
    {
        private PartSupplier candidate;

        private PartSupplier result;

        [SetUp]
        public void SetUp()
        {
            this.candidate = new PartSupplier
                                 {
                                     PartNumber = "PART", 
                                     SupplierId = 1, 
                                     SupplierDesignation = "1234567", 
                                     MinimumOrderQty = 10,
                                     CreatedBy = new Employee { Id = 33087 },
                                     OrderIncrement = 1m,
                                     LeadTimeWeeks = 1,
                                     DateCreated = DateTime.UnixEpoch,
                                     RohsCompliant = "Y",
                                     RohsCategory = "COMPLIANT"
                                 };
            this.MockAuthService.HasPermissionFor(AuthorisedAction.PartSupplierUpdate, Arg.Any<IEnumerable<string>>())
                .Returns(true);
            this.result = this.Sut.CreatePartSupplier(this.candidate, new List<string>());
        }

        [Test]
        public void ShouldReturnCreated()
        {
            this.result.SupplierId.Should().Be(1);
            this.result.PartNumber.Should().Be("PART");
            this.result.SupplierDesignation.Should().Be("1234567");
        }
    }
}
