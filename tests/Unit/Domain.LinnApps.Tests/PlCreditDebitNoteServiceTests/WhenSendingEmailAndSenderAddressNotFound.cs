namespace Linn.Purchasing.Domain.LinnApps.Tests.PlCreditDebitNoteServiceTests
{
    using System.Collections.Generic;
    using System.IO;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSendingEmailAndSenderAddressNotFound : ContextBase
    {
        private ProcessResult result;

        [SetUp]
        public void SetUp()
        {
            this.result = this.Sut.SendEmails(
                new Employee(), 
                new PlCreditDebitNote 
                    { 
                        Supplier = new Supplier
                                       {
                                           SupplierContacts = new List<SupplierContact>
                                                                  {
                                                                      new SupplierContact
                                                                          {
                                                                              EmailAddress = "email@address.com", 
                                                                              IsMainOrderContact = "Y"
                                                                          }
                                                                  }
                                       }
                    },
                null);
        }

        [Test]
        public void ShouldReturnFailResult()
        {
            this.result.Success.Should().Be(false);
            this.result.Message.Should().Be("Cannot find sender email address");
        }
    }
}
