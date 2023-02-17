namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System.Linq;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    public class PartDataSheetValuesRepository : EntityFrameworkRepository<PartDataSheetValues, PartDataSheetValuesKey>
    {
        private readonly ServiceDbContext serviceDbContext;

        public PartDataSheetValuesRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.PartDataSheetValues)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override PartDataSheetValues FindById(PartDataSheetValuesKey key)
        {
            return this.serviceDbContext.PartDataSheetValues.SingleOrDefault(
                x => x.Field == key.Field 
                     && x.AttributeSet == key.AttributeSet 
                     && x.Value == key.Value);
        }
    }
}
