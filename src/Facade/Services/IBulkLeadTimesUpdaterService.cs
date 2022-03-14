namespace Linn.Purchasing.Facade.Services
{
    using Linn.Common.Facade;
    using Linn.Purchasing.Resources;

    public interface IBulkLeadTimesUpdaterService
    {
        IResult<ProcessResultResource> BulkUpdateFromCsv(string csvString);
    }
}
