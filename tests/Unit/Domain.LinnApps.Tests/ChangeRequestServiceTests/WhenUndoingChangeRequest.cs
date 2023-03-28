namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestServiceTests
{
    using System;
    using System.Collections.Generic;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUndoingChangeRequest : ContextBase
    {
        private ChangeRequest result;

        [SetUp]
        public void SetUp()
        {
            var request = new ChangeRequest
                              {
                                  DocumentNumber = 1,
                                  ChangeState = "LIVE",
                                  DateEntered = new DateTime(2022, 1, 1),
                                  DescriptionOfChange = "Brilliant Change Request",
                                  BomChanges = new List<BomChange>
                                                   {
                                                       new BomChange
                                                           {
                                                               ChangeId = 1,
                                                               ChangeState = "LIVE",
                                                               PcasChange = "N"
                                                           },
                                                       new BomChange
                                                           {
                                                               ChangeId = 2,
                                                               ChangeState = "CANCEL",
                                                               PcasChange = "N"
                                                           }
                                                   }
                              };
            this.Repository.FindById(1).Returns(request);

            var employee = new Employee { Id = 7, FullName = "Jane Gullis" };
            this.EmployeeRepository.FindById(7).Returns(employee);

            this.AuthService
                .HasPermissionFor(AuthorisedAction.AdminChangeRequest, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            var selectedBomChangeIds = new List<int> {1};

            this.result = this.Sut.UndoChanges(1, 7, selectedBomChangeIds, null);
        }

        [Test] 
        public void ShouldHaveCalledUndoBomChanges()
        {
            this.BomPack.Received().UndoBomChange(1, 7);
        }
    }
}
