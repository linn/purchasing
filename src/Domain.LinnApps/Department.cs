namespace Linn.Purchasing.Domain.LinnApps
{
    using System.Collections.Generic;

    public class Department
    {
        public string DepartmentCode { get; set; }

        public string Description { get; set; }

        public string ProjectDepartment { get; set; }

        public IEnumerable<NominalAccount> NominalAccounts { get; set; }
    }
}
