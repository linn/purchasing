namespace Linn.Purchasing.Proxy
{
    using System.Data;

    using Linn.Common.Proxy.LinnApps;
    using Oracle.ManagedDataAccess.Client;

    public class MyDatabaseService : IDatabaseService
    {
        public OracleConnection GetConnection()
        {
            OracleConfiguration.TraceLevel = 0;
            return new OracleConnection(ConnectionStrings.ManagedConnectionString());
        }

        public int GetNextVal(string sequenceName)
        {
            using (var connection = this.GetConnection())
            {
                connection.Open();
                var cmd = new OracleCommand("cg_code_controls_next_val", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                var result = new OracleParameter(null, OracleDbType.Int32)
                {
                    Direction = ParameterDirection.ReturnValue,
                    Size = 50
                };
                cmd.Parameters.Add(result);

                var parameter = new OracleParameter("p_cc_domain", OracleDbType.Varchar2)
                {
                    Direction = ParameterDirection.Input,
                    Value = sequenceName
                };
                cmd.Parameters.Add(parameter);

                var num = new OracleParameter("p_increment", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Value = 1
                };
                cmd.Parameters.Add(num);

                cmd.ExecuteNonQuery();
                connection.Close();
                var res = result.Value.ToString();
                return int.Parse(res);
            }
        }

        public DataSet ExecuteQuery(string sql)
        {
            using (var connection = this.GetConnection())
            {
                var dataAdapter = new OracleDataAdapter(
                    new OracleCommand(sql, connection) { CommandType = CommandType.Text });
                var dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                return dataSet;
            }
        }

        public int GetIdSequence(string sequenceName)
        {
            var connection = this.GetConnection();

            var cmd = new OracleCommand("get_next_sequence_value", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            var result = new OracleParameter(string.Empty, OracleDbType.Int32)
            {
                Direction = ParameterDirection.ReturnValue
            };
            cmd.Parameters.Add(result);

            var sequenceParameter = new OracleParameter("p_sequence", OracleDbType.Varchar2)
            {
                Direction = ParameterDirection.Input,
                Size = 50,
                Value = sequenceName
            };
            cmd.Parameters.Add(sequenceParameter);

            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();

            return int.Parse(result.Value.ToString());
        }
    }
}
