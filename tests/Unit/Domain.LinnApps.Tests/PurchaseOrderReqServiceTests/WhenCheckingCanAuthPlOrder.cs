namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderReqServiceTests
{
    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrderReqs;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCheckingCanAuthPlOrder : ContextBase
    {
        private readonly int authoriserUserNumber = 33107;

        private readonly int reqNumber = 5678;

        private PurchaseOrderReq entity;

        private ProcessResult result;

        [SetUp]
        public void SetUp()
        {
            this.entity = new PurchaseOrderReq
                              {
                                  ReqNumber = this.reqNumber,
                                  State = "ORDER WAIT",
                                  ReqDate = 2.March(2022),
                                  OrderNumber = 1234,
                                  PartNumber = "PCAS 007",
                                  Description = "Descrip",
                                  Qty = 7,
                                  UnitPrice = 8m,
                                  Carriage = 99m,
                                  TotalReqPrice = 145m,
                                  CurrencyCode = "SMC",
                                  SupplierId = 111,
                                  SupplierName = "the things shop",
                                  SupplierContact = "Lawrence Chaney",
                                  AddressLine1 = "The shop",
                                  AddressLine2 = "1 Main Street",
                                  AddressLine3 = "town centre",
                                  AddressLine4 = "Glesga",
                                  PostCode = "G1 1AA",
                                  CountryCode = "GB",
                                  PhoneNumber = "+44 1234567780",
                                  QuoteRef = "blah",
                                  Email = "LC@gmail",
                                  DateRequired = 1.January(2023),
                                  RequestedById = 33107,
                                  AuthorisedById = 33107,
                                  SecondAuthById = null,
                                  FinanceCheckById = 33107,
                                  TurnedIntoOrderById = null,
                                  NominalCode = "00001234",
                                  RemarksForOrder = "needed asap",
                                  InternalNotes = "pls approv",
                                  DepartmentCode = "00002345"
                              };

            this.MockCurrencyPack.CalculateBaseValueFromCurrencyValue(
                this.entity.CurrencyCode,
                this.entity.TotalReqPrice.Value).Returns(145m);

            this.MockPurchaseOrdersPack.OrderCanBeAuthorisedBy(
                null,
                null,
                this.authoriserUserNumber,
                145m,
                this.entity.PartNumber,
                "PO").Returns(false);

            this.result = this.Sut.CheckIfSigningLimitCanAuthorisePurchaseOrder(this.entity, this.authoriserUserNumber);
        }

        [Test]
        public void ShouldUpdateFields()
        {
            this.result.Success.Should().BeFalse();
            this.result.Message.Should().Be(
                "Your signing limit will not cover this req (£145). The order will be created unauthorised if you continue");
        }
    }
}
