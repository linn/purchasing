namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderRemindersMailer
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSendingReminder : ContextBase
    {
        private PurchaseOrderDelivery delivery;

        [SetUp]
        public void SetUp()
        {
            this.delivery = new PurchaseOrderDelivery
                                {
                                    OrderNumber = 123,
                                    DateAdvised = 28.March(2023),
                                    OrderDeliveryQty = 100,
                                    PurchaseOrderDetail = new PurchaseOrderDetail
                                                              {
                                                                  SuppliersDesignation = "DESIG",
                                                                  PartNumber = "PART",
                                                                  PurchaseOrder = new PurchaseOrder
                                                                      {
                                                                          Supplier = new Supplier
                                                                              {
                                                                                  Name = "Test Supplier",
                                                                                  SupplierContacts =
                                                                                      new List<SupplierContact>
                                                                                          {
                                                                                              new
                                                                                              SupplierContact
                                                                                                  {
                                                                                                      EmailAddress =
                                                                                                          "supplier@test.com",
                                                                                                      IsMainOrderContact =
                                                                                                          "Y"
                                                                                                  }
                                                                                          },
                                                                                  VendorManager =
                                                                                      new VendorManager
                                                                                          {
                                                                                              Employee =
                                                                                                  new Employee
                                                                                                      {
                                                                                                          FullName =
                                                                                                              "Test Person",
                                                                                                          PhoneListEntry =
                                                                                                              new
                                                                                                              PhoneListEntry
                                                                                                                  {
                                                                                                                      EmailAddress =
                                                                                                                          "test@person.com"
                                                                                                                  }
                                                                                                      }
                                                                                          }
                                                                              }
                                                                      }
                                                              }
                                };
            this.MockRepository
                .FindById(
                    Arg.Is<PurchaseOrderDeliveryKey>(
                        k => k.OrderNumber == 123 && k.OrderLine == 1 && k.DeliverySequence == 1))
                .Returns(this.delivery);

            this.Sut.SendDeliveryReminder(123, 1, 1);
        }

        [Test]
        public void ShouldSendEmail()
        {
            var expectedBody =
                $"Linn sent you a Purchase Order ${this.delivery.OrderNumber} that you have previously confirmed.";
            expectedBody += Environment.NewLine;
            expectedBody += Environment.NewLine;

            expectedBody +=
                "The order is now due for delivery, can you please provide the shipping details including the tracking number.";
            expectedBody += Environment.NewLine;
            expectedBody += Environment.NewLine;

            expectedBody += $"Item: {delivery.PurchaseOrderDetail.SuppliersDesignation}";
            expectedBody += Environment.NewLine;
            expectedBody += $"Linn Part Number: {delivery.PurchaseOrderDetail.PartNumber}";
            expectedBody += Environment.NewLine;
            expectedBody += $"Delivery Qty: {delivery.OrderDeliveryQty}";
            expectedBody += Environment.NewLine;
            expectedBody += $"Promise Date: {delivery.DateAdvised.GetValueOrDefault():dd-MM-yyyy}";
            expectedBody += Environment.NewLine;
            expectedBody += Environment.NewLine;
            expectedBody += "Kind Regards,";
            expectedBody += Environment.NewLine;
            expectedBody += "Test Person";
            expectedBody += Environment.NewLine;
            expectedBody += "Linn Products Ltd";

            this.MockEmailService.Received(1).SendEmail(
                "supplier@test.com",
                "Test Supplier",
                null,
                null,
                "test@person.com",
                "Test Person",
                "LINN PRODUCTS PURCHASE ORDER DELIVERY REMINDER",
                expectedBody,
                null);
        }


        [Test]
        public void ShouldMarkDeliveryAsSent()
        {
            this.delivery.ReminderSent.Should().Be("Y");
        }
    }
}
