namespace Linn.Purchasing.Integration.Tests.SupplierModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;
    using NSubstitute.ReceivedExtensions;

    using NUnit.Framework;

    public class WhenPuttingSupplierOnHold : ContextBase
    {
        private SupplierHoldChangeResource resource;

        [SetUp]
        public void SetUp()
        {
            this.resource = new SupplierHoldChangeResource
                                {
                                    SupplierId = 1,
                                    PutOnHoldBy = 33087,
                                    ReasonOnHold = "HOLDING TIGHT"
                                };
            this.MockDatabaseService.GetIdSequence("SOHH_SEQ").Returns(123);
            this.MockDomainService
                .ChangeSupplierHoldStatus(Arg.Any<SupplierOrderHoldHistoryEntry>(), Arg.Any<IEnumerable<string>>())
                .Returns(new Supplier { SupplierId = 1 });

            this.Response = this.Client.Post(
                $"/purchasing/suppliers/hold",
                this.resource,
                with =>
                    {
                        with.Accept("application/json");
                    }).Result;
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldGetSeqNextVal()
        {
            this.MockDatabaseService.Received().GetIdSequence("SOHH_SEQ");
        }

        [Test]
        public void ShouldCallDomainService()
        {
            this.MockDomainService.Received().ChangeSupplierHoldStatus(
                Arg.Is<SupplierOrderHoldHistoryEntry>(e => 
                    e.SupplierId == 1 
                    && e.PutOnHoldBy == 33087 
                    && e.ReasonOnHold == this.resource.ReasonOnHold),
                Arg.Any<IEnumerable<string>>());
        }

        [Test]
        public void ShouldCommit()
        {
            this.TransactionManager.Received().Commit();
        }

        [Test]
        public void ShouldReturnJsonResult()
        {
            var resultResource = this.Response.DeserializeBody<SupplierResource>();
            resultResource.Should().NotBeNull();
            resultResource.Id.Should().Be(1);
            resultResource.Links.Any(l => l.Rel == "self").Should().BeTrue();
        }
    }
}
