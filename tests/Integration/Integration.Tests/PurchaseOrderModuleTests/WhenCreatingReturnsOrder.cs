namespace Linn.Purchasing.Integration.Tests.PurchaseOrderModuleTests
{
    using System.Collections.Generic;
    using System.Net.Http.Json;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenReturnOrder : ContextBase
    {
        private PurchaseOrderResource resource;

        [SetUp]
        public void SetUp()
        {
            this.resource = new PurchaseOrderResource
            {
                OrderNumber = 600179,
                Supplier = new SupplierResource(),
                Cancelled = "N",
                DocumentType = new DocumentTypeResource { Description = "order", Name = "RO" },
                OrderDate = 1.January(2022).ToString("O"),
                OverbookQty = null,
                Details = new List<PurchaseOrderDetailResource>
                              { 
                                  new PurchaseOrderDetailResource
                                    { 
                                        OrderPosting 
                                            = new PurchaseOrderPostingResource
                                                  {
                                                      NominalAccount =
                                                          new NominalAccountResource(),
                                                         }
                                    }
                              },
                Currency = new CurrencyResource(),
                OrderMethod = new OrderMethodResource(),

                DeliveryAddress = new LinnDeliveryAddressResource(),
                RequestedBy = new EmployeeResource(),
                EnteredBy = new EmployeeResource { Id = 1 },
                AuthorisedBy = new EmployeeResource(),
                OrderAddress = new AddressResource()
            };

            this.MockSupplierRepository.FindById(Arg.Any<int>())
                .Returns(new Supplier());

            this.MockNominalAccountRepository.FindById(Arg.Any<int>())
                .Returns(new NominalAccount());

            this.MockDomainService.CreateOrder(
                    Arg.Any<PurchaseOrder>(), Arg.Any<IEnumerable<string>>())
                .Returns(new PurchaseOrder { DocumentType = new DocumentType { Name = "RO" }, DocumentTypeName = "RO" });

            this.Response = 
                this.Client.PostAsJsonAsync("/purchasing/purchase-orders", this.resource).Result;
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resultResource = this.Response.DeserializeBody<PurchaseOrderResource>();
            resultResource.Should().NotBeNull();
        }

        [Test]
        public void ShouldNotCallCreateDebitNote()
        {
            this.MockPlCreditDebitNoteService.Received().CreateDebitOrNoteFromPurchaseOrder(
                Arg.Any<PurchaseOrder>());
        }
    }
}
