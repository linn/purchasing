namespace Linn.Purchasing.Domain.LinnApps
{
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    using Linn.Common.Pdf;

    public class HtmlTemplateService<T> : IHtmlTemplateService<T>
    {
        private readonly string pathToTemplate;

        private readonly ITemplateEngine templateEngine;

        public HtmlTemplateService(
            string pathToTemplate,
            ITemplateEngine templateEngine)
        {
            this.pathToTemplate = pathToTemplate;
            this.templateEngine = templateEngine;
        }

        public async Task<string> GetHtml(T data)
        {
            var template = await new StreamReader(this.pathToTemplate, Encoding.UTF8).ReadToEndAsync();
            var html = await this.templateEngine.Render(data, template);

            return html;
        }
    }
}
