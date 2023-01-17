namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System.Collections.Generic;
    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using System;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Common.Domain.Exceptions;
    using Linn.Common.Proxy.LinnApps;

    public class BomVerificationHistoryService : IBomVerificationHistoryService
    {
        private readonly IAuthorisationService authService;

        private readonly IDatabaseService databaseService;

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

        public BomVerificationHistory GetHistoryEntry(BomVerificationHistory bomHistoryEntry)
        {
            var bomVerificationHistoryEntry = this.bomVerificationRepository.FindById((int)bomHistoryEntry.DocumentNumber);
            if (bomVerificationHistoryEntry is null)
            {
                throw new ItemNotFoundException($"Could not find order {bomHistoryEntry.DocumentNumber}");
            }

            return bomVerificationHistoryEntry;
        }

        private Part ValidPartNumber(string partNumber)
        {
            if (string.IsNullOrEmpty(partNumber))
            {
                return null;
            }

            var part = this.partRepository.FindBy(p => p.PartNumber == partNumber);
            if (part == null)
            {
                throw new DomainException("invalid part number");
            }

            return part;
        }

        public BomVerificationHistory CreateBomVerificationHistory(BomVerificationHistory bomHistoryEntry)
        {
            var employee = this.employeeRepository.FindById(bomHistoryEntry.VerifiedBy);
            if (employee == null)
            {
                throw new ItemNotFoundException("Employee not found");
            }

            var bomVerificationHistoryEntry = new BomVerificationHistory
            {
                TRef = this.databaseService.GetIdSequence("BVH_SEQ"),
                DocumentNumber = bomHistoryEntry.DocumentNumber,
                DateVerified = DateTime.Now.ToString("O"),
                PartNumber = bomHistoryEntry.PartNumber,
                DocumentType= bomHistoryEntry.DocumentType,
                Remarks= bomHistoryEntry.Remarks,
                VerifiedBy = bomHistoryEntry.VerifiedBy
            };

            this.bomVerificationRepository.Add(bomVerificationHistoryEntry);

            return bomVerificationHistoryEntry;
        }
    }
}
