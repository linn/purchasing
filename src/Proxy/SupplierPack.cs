namespace Linn.Purchasing.Proxy
{
    using System;
    using System.Data;

    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;

    using Oracle.ManagedDataAccess.Client;

    public class SupplierPack : ISupplierPack
    {
        private readonly IDatabaseService databaseService;

        public SupplierPack(IDatabaseService databaseService)
        {
            this.databaseService = databaseService;
        }

        public int GetNextSupplierKey()
        {
            using var connection = this.databaseService.GetConnection();

            connection.Open();
            var cmd = new OracleCommand("supp_pack.get_next_supplier_key", connection)
                          {
                              CommandType = CommandType.StoredProcedure
                          };
            var result = new OracleParameter(null, OracleDbType.Int32)
                             { 
                                 Direction = ParameterDirection.ReturnValue
                             }; 
            cmd.Parameters.Add(result);

            cmd.ExecuteNonQuery();
            connection.Close();

            return int.Parse(result.Value.ToString());
        }
    }
}
