namespace Linn.Purchasing.Facade
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    public interface IHandler2
    {
        bool CanHandle(object model, string contentType);

        Task Handle(HttpRequest req, HttpResponse res, object model, CancellationToken cancellationToken);
    }
}
