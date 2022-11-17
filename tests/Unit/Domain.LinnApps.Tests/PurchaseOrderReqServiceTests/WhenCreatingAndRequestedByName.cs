namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderReqServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrderReqs;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreatingAndRequestedByName : ContextBase
    {
        private PurchaseOrderReq candidate;

        private PurchaseOrderReq result;

        [SetUp]
        public void SetUp()
        {
            this.candidate = new PurchaseOrderReq
                                 {
                                     State = "AUTHORISE WAIT",
                                     AuthorisedById = 12345,
                                     PartNumber = "PART",
                                     SupplierId = 54321,
                                     RequestedById = 33087,
                                     RemarksForOrder = "Something Remarkable."
                                 };

            this.MockSupplierRepository.FindById(this.candidate.SupplierId).Returns(
                new Supplier
                    {
                        DateClosed = null,
                        OrderHold = "N"
                    });
            this.MockPartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>())
                .Returns(new Part { StockControlled = "N" });
            this.EmployeeRepository.FindById(this.candidate.RequestedById).Returns(
                new Employee { FullName = "SOME PERSON" });
            this.result = this.Sut.Create(this.candidate, new List<string>());
        }

        [Test]
        public void ShouldAppendFaoRemark()
        {
            this.result.RemarksForOrder.Should().Be(
                $"Please send for the attention of SOME PERSON. {Environment.NewLine}Something Remarkable.");
        }
    }
}
