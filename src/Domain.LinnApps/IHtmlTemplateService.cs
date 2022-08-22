namespace Linn.Purchasing.Domain.LinnApps
{
    using System.Threading.Tasks;

    public interface IHtmlTemplateService<in T>
    {
        Task<string> GetHtml(T data);
    }
}
