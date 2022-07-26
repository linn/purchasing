namespace Linn.Purchasing.Facade
{
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    public class FileReader : IFileReader
    {
        public async Task<string> ReadFile(string path)
        {
            return await new StreamReader(
                path,
                Encoding.UTF8).ReadToEndAsync();
        }
    }
}
