namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Common.Domain.Exceptions;

    public class BomVerificationHistoryService : IBomVerificationHistoryService
    {
        private readonly IAuthorisationService authService;

        private readonly IRepository<BomVerificationHistory, int> bomVerificationRepository;

        private readonly IQueryRepository<Part> partRepository;

        private readonly IRepository<Employee, int> employeeRepository;

        public BomVerificationHistoryService(IAuthorisationService authService, 
            IQueryRepository<Part> partRepository, 
            IRepository<BomVerificationHistory, int> bomVerificationRepository, 
            IRepository<Employee, int> employeeRepository)

        {
            this.authService = authService;
            this.partRepository = partRepository;
            this.bomVerificationRepository = bomVerificationRepository;
            this.employeeRepository = employeeRepository;
        }
    }
}
