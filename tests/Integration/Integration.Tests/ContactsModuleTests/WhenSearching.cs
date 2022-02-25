namespace Linn.Purchasing.Integration.Tests.ContactsModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSearching : ContextBase
    {
        private string searchTerm;

        private List<Contact> dataResult;

        [SetUp]
        public void SetUp()
        {
            this.searchTerm = "T1";

            this.dataResult = new List<Contact>
                                  {
                                      new Contact
                                          {
                                              ContactId = 1,
                                              EmailAddress = "email@address.com",
                                              PhoneNumber = "012345",
                                              Person = new Person
                                                           {
                                                               FirstName = "MR",
                                                               LastName = "CONTACT"
                                                           }
                                          }
                                  };

            this.MockContactRepository.FilterBy(Arg.Any<Expression<Func<Contact, bool>>>())
                .Returns(this.dataResult.AsQueryable());
            this.Response = this.Client.Get(
                $"/purchasing/suppliers/contacts?searchTerm={this.searchTerm}",
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
        public void ShouldReturnJsonContentType()
        {
            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resources = this.Response.DeserializeBody<IEnumerable<ContactResource>>()?.ToArray();
            resources.Should().NotBeNull();
            resources.Should().HaveCount(1);
            var expected = this.dataResult.First();
            var contact = resources.First();
            contact.ContactId.Should().Be(expected.ContactId);
            contact.EmailAddress.Should().Be(expected.EmailAddress);
            contact.FirstName.Should().Be(expected.Person.FirstName);
            contact.LastName.Should().Be(expected.Person.LastName);
        }
    }
}
