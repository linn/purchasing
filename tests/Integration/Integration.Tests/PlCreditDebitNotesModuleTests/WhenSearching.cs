namespace Linn.Purchasing.Integration.Tests.PlCreditDebitNotesModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSearching : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.MockPlCreditDebitNoteRepository.FilterBy(Arg.Any<Expression<Func<PlCreditDebitNote, bool>>>())
                .Returns(new List<PlCreditDebitNote>
                             {
                                 new PlCreditDebitNote
                                     {
                                         NoteNumber = 1, 
                                         Supplier = new Supplier { Name = "SUPPLIER" }
                                     },
                                 new PlCreditDebitNote
                                     {
                                         NoteNumber = 2,  
                                         Supplier = new Supplier { Name = "SUPPLIER 2" }
                                     }
                             }.AsQueryable());
            this.Response = this.Client.Get(
                "/purchasing/pl-credit-debit-notes?searchTerm=supplier",
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
            var resources = this.Response.DeserializeBody<IEnumerable<PlCreditDebitNoteResource>>()?.ToArray();
            resources.Should().NotBeNull();
            resources.Should().HaveCount(2);
            resources.Should().Contain(a => a.NoteNumber == 1);
            resources.Should().Contain(a => a.NoteNumber == 2);
        }
    }
}
