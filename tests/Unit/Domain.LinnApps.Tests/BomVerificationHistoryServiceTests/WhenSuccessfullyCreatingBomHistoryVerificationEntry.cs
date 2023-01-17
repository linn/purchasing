namespace Linn.Purchasing.Domain.LinnApps.Tests.BomVerificationHistoryServiceTests
{
    using System;
    using System.Linq.Expressions;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenSuccessfullyCreatingBomHistoryVerificationEntry : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            var employee = new Employee { Id = 100, FullName = "B.Slime" };
            this.EmployeeRepository.FindById(100).Returns(employee);

            var request = new BomVerificationHistory
                         {
                                  TRef = 123,
                                  DateVerified= new DateTime(2023, 1, 1).ToString("o"),    
                                  DocumentNumber = 123456,
                                  DocumentType = "Test",
                                  PartNumber = "CAP 500",
                                  Remarks = "Big testing",
                                  VerifiedBy = 100

            };
            this.Sut.CreateBomVerificationHistory(request);
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.BomVerificationHistoryRepository
                .Received(1).Add(Arg.Any<BomVerificationHistory>());
        }
    }
}
