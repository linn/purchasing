namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System.Linq;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Purchasing.Domain.LinnApps;

    using Microsoft.EntityFrameworkCore;

    public class EmployeeRepository : EntityFrameworkRepository<Employee, int>
    {
        public EmployeeRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.Employees)
        {
        }

        public override Employee FindById(int key)
        {
            return this.FindAll().Include(e => e.PhoneListEntry).SingleOrDefault(e => e.Id == key);
        }
    }
}
