namespace Linn.Purchasing.Domain.LinnApps
{
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    using RazorEngineCore;

    public class RazorTemplateService : IRazorTemplateService
    {
        private readonly IRazorEngine razorEngine;

        public RazorTemplateService(IRazorEngine razorEngine)
        {
            this.razorEngine = razorEngine;
        }

        public Task<string> Render<T>(T model, string fileUrl)
        {
            using (var file = new StreamReader(fileUrl, Encoding.UTF8))
            {
                var fileRead = file.ReadToEnd();

                IRazorEngineCompiledTemplate<RazorEngineTemplateBase<T>> template =
                    this.razorEngine.Compile<RazorEngineTemplateBase<T>>(fileRead);

                var result = template.RunAsync(instance => { instance.Model = model; });

                return result;
            }
        }
    }
}
