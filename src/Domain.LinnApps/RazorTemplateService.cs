namespace Linn.Purchasing.Domain.LinnApps
{
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    using RazorEngineCore;

    public class RazorTemplateService : IRazorTemplateService
    {
        public Task<string> Render<T>(T model, string fileUrl)
        {
            using (var file = new StreamReader(fileUrl, Encoding.UTF8))
            {
                var fileRead = file.ReadToEnd();
                var razorEngine = new RazorEngine();

                IRazorEngineCompiledTemplate<RazorEngineTemplateBase<T>> template =
                    razorEngine.Compile<RazorEngineTemplateBase<T>>(fileRead);

                var result = template.RunAsync(instance => { instance.Model = model; });

                return result;
            }
        }
    }
}
