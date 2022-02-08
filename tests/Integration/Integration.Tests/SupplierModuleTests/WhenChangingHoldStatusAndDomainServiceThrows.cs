namespace Linn.Purchasing.Integration.Tests.SupplierModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;
    using NSubstitute.ExceptionExtensions;

    using NUnit.Framework;

    public class WhenChangingHoldStatusAndDomainServiceThrows : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.MockDomainService.ChangeSupplierHoldStatus(
                Arg.Any<SupplierOrderHoldHistoryEntry>(),
                Arg.Any<IEnumerable<string>>()).Throws(new Exception("Something went wrong"));

            this.Response = this.Client.Post(
                $"/purchasing/suppliers/hold",
                 new SupplierHoldChangeResource
                 {
                     SupplierId = 1,
                     TakenOffHoldBy = 33087,
                     ReasonOffHold = "HOLDING TIGHT"
                 },
                with =>
                    {
                        with.Accept("application/json");
                    }).Result;
        }

        [Test]
        public void ShouldReturnBadRequest()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
