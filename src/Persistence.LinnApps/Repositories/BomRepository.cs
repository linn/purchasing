namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Boms;

    using Microsoft.EntityFrameworkCore;

    public class BomRepository : IRepository<Bom, int>
    {
        private readonly ServiceDbContext serviceDbContext;

        public BomRepository(ServiceDbContext serviceDbContext)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public Bom FindById(int key)
        {
            return this.serviceDbContext.Boms.Where(b => b.BomId == key)
                .Include(b => b.Details.Where(d => d.ChangeState == "LIVE")
                    .OrderBy(d => d.PartNumber))
                .ThenInclude(d => d.Part).FirstOrDefault();
        }

        public IQueryable<Bom> FindAll()
        {
            throw new NotImplementedException();
        }

        public void Add(Bom entity)
        {
            this.serviceDbContext.Add(entity);
        }

        public void Remove(Bom entity)
        {
            throw new NotImplementedException();
        }

        public Bom FindBy(Expression<Func<Bom, bool>> expression)
        {
            return this.serviceDbContext
                .Boms.AsNoTracking().Where(expression)
                .Include(b => b.Part)
                .Include(b => b.Details)
                .ThenInclude(d => d.Part).ThenInclude(p => p.PartSuppliers.Where(ps => ps.SupplierRanking == 1))
                .Include(b => b.Details)
                .ThenInclude(d => d.PartRequirement)
                .Include(b => b.Details)
                .ThenInclude(d => d.AddChange)
                .Include(b => b.Details)
                .ThenInclude(d => d.DeleteChange).FirstOrDefault();
        }

        public IQueryable<Bom> FilterBy(Expression<Func<Bom, bool>> expression)
        {
            throw new NotImplementedException();
        }
    }
}
