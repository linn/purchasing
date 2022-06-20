namespace Linn.Purchasing.Domain.LinnApps.Edi
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;

    public class EdiOrderService : IEdiOrderService
    {
        private readonly IEdiEmailPack ediEmailPack;

        private readonly IRepository<EdiOrder, int> repository;

        private readonly IRepository<EdiSupplier, int> supplierRepository;

        public EdiOrderService(IEdiEmailPack ediEmailPack, IRepository<EdiOrder, int> repository, IRepository<EdiSupplier, int> supplierRepository)
        {
            this.ediEmailPack = ediEmailPack;
            this.repository = repository;
            this.supplierRepository = supplierRepository;
        }

        public IEnumerable<EdiOrder> GetEdiOrders(int supplierId)
        {
            this.ediEmailPack.GetEdiOrders(supplierId);

            var orders = this.repository.FilterBy(o => o.SupplierId == supplierId && o.SequenceNumber == null);

            return orders;
        }

        public IEnumerable<EdiSupplier> GetEdiSuppliers()
        {
            var suppliers = this.supplierRepository.FindAll();

            return suppliers;
        }

        public ProcessResult SendEdiOrder(int supplierId, string altEmail, string additionalEmail, string additionalText, bool test)
        {
            try
            {
                var result = this.ediEmailPack.SendEdiOrder(supplierId, altEmail, additionalEmail, additionalEmail, test);
                return new ProcessResult(result.StartsWith("SUCCESS"), result );
            }
            catch (Exception e)
            {
                return new ProcessResult(false, e.Message);
            }
        }
    }
}

