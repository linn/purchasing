namespace Linn.Purchasing.Facade.Services
{
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    public class PartService : IPartService
    {
        private readonly IQueryRepository<Part> partRepository;

        public PartService(IQueryRepository<Part> partRepository)
        {
            this.partRepository = partRepository;
        }

        public string GetPartNumberFromId(int id)
        {
            return this.partRepository.FindBy(x => x.Id == id)?.PartNumber;
        }
    }
}
