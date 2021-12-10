namespace Linn.Purchasing.Domain.LinnApps.PartSuppliers
{
    using System.Collections.Generic;

    using Linn.Common.Authorisation;

    public class PartSupplierService : IPartSupplierService
    {
        private readonly IAuthorisationService authService;

        public PartSupplierService(IAuthorisationService authService)
        {
            this.authService = authService;
        }

        public void UpdatePartSupplier(PartSupplier current, PartSupplier updated, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PartSupplierUpdate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to update Part Supplier records");
            }

            current.SupplierDesignation = updated.SupplierDesignation;
        }

        public PartSupplier CreatePartSupplier(PartSupplier candidate, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PartSupplierUpdate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to update Part Supplier records");
            }

            return candidate;
        }
    }
}
