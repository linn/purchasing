namespace Linn.Purchasing.Domain.LinnApps
{
    using System.Threading.Tasks;

    public interface IRazorTemplateService
    {
        Task<string> Render<T>(T model, string fileUrl);
    }
}
