namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderServiceTests
{
    using System;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenEmailingDeptAndOrderIsNotAuthorised : ContextBase
    {
        private PurchaseOrder order;

        private Employee user;

        private Supplier supplier;

        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.user = new Employee
            {
                Id = 123,
                PhoneListEntry = new PhoneListEntry()
                {
                    EmailAddress = "test@user.com"
                },
                FullName = "USER MCUSERSON"
            };
            this.supplier = new Supplier { Name = "SUPPLIER" };
            this.order = new PurchaseOrder
            {
                OrderNumber = 321,
                AuthorisedById = null,
                Supplier = this.supplier,
                EnteredBy = new Employee
                {
                    PhoneListEntry = new PhoneListEntry
                    {
                        EmailAddress = "purchasing@shop.com"
                    }
                }
            };

            this.PurchaseOrderRepository.FindById(this.order.OrderNumber).Returns(this.order);
            this.EmployeeRepository.FindById(this.user.Id).Returns(this.user);

            this.action = () => this.Sut.EmailDept(this.order.OrderNumber, this.user.Id);
        }

        [Test]
        public void ShouldThrowException()
        {
            this.action.Should().Throw<PurchaseOrderException>();
        }
    }
}
