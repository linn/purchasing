namespace Linn.Purchasing.Domain.LinnApps.ExternalServices
{
    public interface IStoresMatVarPack
    {
        int MakeReqHead(int who);

        void MakeReqLine(int reqNumber, string partNumber, int who);
    }
}
