namespace Linn.Purchasing.Integration.Tests.SupplierModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

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
                                    Contacts = new List<SupplierContactResource>
                                                   {
                                                       new SupplierContactResource
                                                           {
                                                               IsMainInvoiceContact = "Y",
                                                               IsMainOrderContact = "N",
                                                               Contact = new ContactResource
                                                                             {
                                                                                 DateCreated = DateTime.UnixEpoch.ToString("o"),
                                                                                 Comments = "COMMENT",
                                                                                 EmailAddress = "email@address.com",
                                                                                 FirstName = "Contact",
                                                                                 LastName = "Resource",
                                                                                 JobTitle = "Contact Person",
                                                                                 PersonId = 1,
                                                                                 MobileNumber = "0123456",
                                                                                 PhoneNumber = "09876"
                                                                             }
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
            this.Response = this.Client.Put(
                $"/purchasing/suppliers/{this.resource.Id}",
                this.resource,
                with => { with.Accept("application/json"); }).Result;
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
            var resourceContact = this.resource.Contacts.First().Contact;
            this.MockDomainService.Received().UpdateSupplier(
                Arg.Is<Supplier>(s => s.SupplierId == 1  && s.Name == "SUPPLIER"),
                Arg.Is<Supplier>(s => s.SupplierId == 1 
                                      && s.Name == "NEW NAME"
                                      && s.Contacts.First().IsMainInvoiceContact == "Y"
                                      && s.Contacts.First().IsMainOrderContact == "N"
                                      && s.Contacts.First().Contact.DateCreated == DateTime.UnixEpoch
                                      && s.Contacts.First().Contact.Comments == resourceContact.Comments
                                      && s.Contacts.First().Contact.EmailAddress == resourceContact.EmailAddress
                                      && s.Contacts.First().Contact.Person.FirstName == resourceContact.FirstName
                                      && s.Contacts.First().Contact.Person.LastName == resourceContact.LastName
                                      && s.Contacts.First().Contact.Person.Id == resourceContact.PersonId
                                      && s.Contacts.First().Contact.JobTitle == resourceContact.JobTitle
                                      && s.Contacts.First().Contact.PhoneNumber == resourceContact.PhoneNumber
                                      && s.Contacts.First().Contact.MobileNumber == resourceContact.MobileNumber),
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
