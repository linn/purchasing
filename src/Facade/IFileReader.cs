namespace Linn.Purchasing.Facade
{
    using System.Threading.Tasks;

    public interface IFileReader
    {
        public Task<string> ReadFile(string path);
    }
}
