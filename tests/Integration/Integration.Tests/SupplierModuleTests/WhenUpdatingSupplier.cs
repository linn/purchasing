namespace Linn.Purchasing.Integration.Tests.SupplierModuleTests
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

    public class WhenUpdatingSupplier : ContextBase
    {
        private SupplierResource resource;

        private Supplier supplier;

        [SetUp]
        public void SetUp()
        {

            this.resource = new SupplierResource
                                {
                                    Id = 1, 
                                    Name = "NEW NAME",
                                    SupplierContacts = new List<SupplierContactResource>
                                                   {
                                                       new SupplierContactResource
                                                           {
                                                               IsMainInvoiceContact = "Y",
                                                               IsMainOrderContact = "N",
                                                               EmailAddress = "email@address.com",
                                                               FirstName = "Contact",
                                                               LastName = "Resource",
                                                               MobileNumber = "0123456",
                                                               PhoneNumber = "09876",
                                                               Comments = "COMMENT",
                                                               PersonId = 1,
                                                               JobTitle = "CONTACT"
                                                           }
                                                   }
                                };
            this.supplier = new Supplier
                                {
                                    SupplierId = 1,
                                    Name = "SUPPLIER",
                                    OpenedBy = new Employee { Id = 1 }
                                };
            this.MockSupplierRepository.FindById(1).Returns(this.supplier);
            this.Response = this.Client.PutAsJsonAsync(
                $"/purchasing/suppliers/{this.resource.Id}",
                this.resource).Result;
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldReturnJsonContentType()
        {
            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldCallDomainService()
        {
            var resourceContact = this.resource.SupplierContacts.First();
            this.MockDomainService.Received().UpdateSupplier(
                Arg.Is<Supplier>(s => s.SupplierId == 1  && s.Name == "SUPPLIER"),
                Arg.Is<Supplier>(s => s.SupplierId == 1 
                                      && s.Name == "NEW NAME"
                                      && s.SupplierContacts.First().IsMainInvoiceContact == "Y"
                                      && s.SupplierContacts.First().IsMainOrderContact == "N"
                                      && s.SupplierContacts.First().Comments == resourceContact.Comments
                                      && s.SupplierContacts.First().EmailAddress == resourceContact.EmailAddress
                                      && s.SupplierContacts.First().Person.Id == resourceContact.PersonId
                                      && s.SupplierContacts.First().JobTitle == resourceContact.JobTitle
                                      && s.SupplierContacts.First().PhoneNumber == resourceContact.PhoneNumber
                                      && s.SupplierContacts.First().MobileNumber == resourceContact.MobileNumber),
                Arg.Any<IEnumerable<string>>());
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resultResource = this.Response.DeserializeBody<SupplierResource>();
            resultResource.Should().NotBeNull();
            resultResource.Id.Should().Be(1);
        }
    }
}
