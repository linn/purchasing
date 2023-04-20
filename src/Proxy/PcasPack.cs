namespace Linn.Purchasing.Proxy
{
    using System.Data;

    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;

    using Oracle.ManagedDataAccess.Client;

    public class PcasPack : IPcasPack
    {
        private readonly IDatabaseService databaseService;

        public PcasPack(IDatabaseService databaseService)
        {
            this.databaseService = databaseService;
        }

        public string DiscrepanciesOnChange(string boardCode, string revisionCode, int changeId)
        {
            using (var connection = this.databaseService.GetConnection())
            {

                connection.Open();
                var cmd = new OracleCommand("pcas_pack.discrepancies_on_pcaschange", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                var result = new OracleParameter(null, OracleDbType.Varchar2)
                {
                    Size = 4000,
                    Direction = ParameterDirection.ReturnValue
                };

                cmd.Parameters.Add(result);

                cmd.Parameters.Add(
                    new OracleParameter("p_board_code", OracleDbType.Varchar2)
                    {
                        Direction = ParameterDirection.Input,
                        Value = boardCode
                    });
                cmd.Parameters.Add(
                    new OracleParameter("p_revision_code", OracleDbType.Varchar2)
                    {
                        Direction = ParameterDirection.Input,
                        Value = revisionCode
                    });
                cmd.Parameters.Add(
                    new OracleParameter("p_change_id", OracleDbType.Int32)
                    {
                        Direction = ParameterDirection.Input,
                        Value = changeId
                    });
                cmd.ExecuteNonQuery();
                connection.Close();


                if (result.Value is null || result.Value?.ToString() == "null")
                {
                    return string.Empty;
                }

                return result.Value.ToString();
            }
        }

        public void UndoPcasChange(int changeId, int undoneBy)
        {
            using (var connection = this.databaseService.GetConnection())
            {
                connection.Open();
                var cmd = new OracleCommand("pcas_pack.undo_pcas_change", connection)
                              {
                                  CommandType = CommandType.StoredProcedure
                              };

                cmd.Parameters.Add(
                    new OracleParameter("p_change_id", OracleDbType.Int32)
                        {
                            Direction = ParameterDirection.Input,
                            Value = changeId
                        });

                cmd.Parameters.Add(
                    new OracleParameter("p_undone_by", OracleDbType.Int32)
                        {
                            Direction = ParameterDirection.Input,
                            Value = undoneBy
                        });

                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void ReplaceAll(string partNumber, int documentNumber, string changeState, int replacedBy, string newPartNumber)
        {
            /*
             *  	PROCEDURE REPLACE_ALL	  		  (p_part_number in varchar2,
                                       p_document_type in varchar2,
                                       p_document_number in number,

                                       p_change_State in varchar2,
                                       p_entered_by in number,

                                       p_new_part_number in varchar2);
             */
            using (var connection = this.databaseService.GetConnection())
            {
                connection.Open();
                var cmd = new OracleCommand("pcas_pack.replace_all", connection)
                              {
                                  CommandType = CommandType.StoredProcedure
                              };

                cmd.Parameters.Add(
                    new OracleParameter("p_part_number", OracleDbType.Varchar2)
                        {
                            Direction = ParameterDirection.Input,
                            Value = partNumber
                    });

                cmd.Parameters.Add(
                    new OracleParameter("p_document_type", OracleDbType.Varchar2)
                        {
                            Direction = ParameterDirection.Input,
                            Value = "CRF"
                        });

                cmd.Parameters.Add(
                    new OracleParameter("p_document_number", OracleDbType.Int32)
                        {
                            Direction = ParameterDirection.Input,
                            Value = documentNumber
                    });

                cmd.Parameters.Add(
                    new OracleParameter("p_change_state", OracleDbType.Varchar2)
                        {
                            Direction = ParameterDirection.Input,
                            Value = changeState
                        });

                cmd.Parameters.Add(
                    new OracleParameter("p_entered_by", OracleDbType.Int32)
                        {
                            Direction = ParameterDirection.Input,
                            Value = replacedBy
                        });

                cmd.Parameters.Add(
                    new OracleParameter("p_new_part_number", OracleDbType.Varchar2)
                        {
                            Direction = ParameterDirection.Input,
                            Value = newPartNumber
                        });

                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void ReplacePartWith(
            string boardCode,
            string revisionCode,
            int layoutSeq,
            int versionNumber,
            int documentNumber,
            string changeState,
            int enteredBy,
            string cref,
            string partNumber,
            string newPartNumber,
            decimal? newQty,
            string newAsst)
        {
            /*
             PROCEDURE REPLACE_PART_WITH	  (p_board_code in varchar2,
                                                                     p_revision_code in varchar2,
                                                                     p_layout_seq in number,
                                                                     p_version_number in number,
                                       p_document_type in varchar2,

                                       p_document_number in number,

                                       p_change_State in varchar2,

                                       p_entered_by in number,
                                       p_cref in varchar2,
                                       p_part_number in varchar2,
                                       p_new_part_number in varchar2,
                                       p_new_qty in number,
                                       p_new_asst in varchar2);
             */

            using (var connection = this.databaseService.GetConnection())
            {
                connection.Open();
                var cmd = new OracleCommand("pcas_pack.REPLACE_PART_WITH", connection)
                              {
                                  CommandType = CommandType.StoredProcedure
                              };


                cmd.Parameters.Add(
                    new OracleParameter("p_board_code", OracleDbType.Varchar2)
                        {
                            Direction = ParameterDirection.Input,
                            Value = boardCode
                        });

                cmd.Parameters.Add(
                    new OracleParameter("p_revision_code", OracleDbType.Varchar2)
                        {
                            Direction = ParameterDirection.Input, Value = revisionCode
                    });


                cmd.Parameters.Add(
                    new OracleParameter("p_layout_seq", OracleDbType.Int32)
                        {
                            Direction = ParameterDirection.Input,
                            Value = layoutSeq
                        });

                cmd.Parameters.Add(
                    new OracleParameter("p_version_number", OracleDbType.Int32)
                        {
                            Direction = ParameterDirection.Input,
                            Value = versionNumber
                        });

                cmd.Parameters.Add(
                    new OracleParameter("p_document_type", OracleDbType.Varchar2)
                        {
                            Direction = ParameterDirection.Input, Value = "CRF"
                        });

                cmd.Parameters.Add(
                    new OracleParameter("p_document_number", OracleDbType.Int32)
                        {
                            Direction = ParameterDirection.Input, Value = documentNumber
                        });

                cmd.Parameters.Add(
                    new OracleParameter("p_change_state", OracleDbType.Varchar2)
                        {
                            Direction = ParameterDirection.Input, Value = changeState
                        });

                cmd.Parameters.Add(
                    new OracleParameter("p_entered_by", OracleDbType.Int32)
                        {
                            Direction = ParameterDirection.Input,
                            Value = enteredBy
                        });

                cmd.Parameters.Add(
                    new OracleParameter("p_cref", OracleDbType.Varchar2)
                        {
                            Direction = ParameterDirection.Input,
                            Value = cref
                        });

                cmd.Parameters.Add(
                    new OracleParameter("p_part_number", OracleDbType.Varchar2)
                        {
                            Direction = ParameterDirection.Input,
                            Value = partNumber
                        });

                cmd.Parameters.Add(
                    new OracleParameter("p_new_part_number", OracleDbType.Varchar2)
                        {
                            Direction = ParameterDirection.Input, Value = newPartNumber
                        });

                cmd.Parameters.Add(
                    new OracleParameter("p_new_qty", OracleDbType.Decimal)
                        {
                            Direction = ParameterDirection.Input,
                            Value = newQty
                        });

                cmd.Parameters.Add(
                    new OracleParameter("p_new_asst", OracleDbType.Varchar2)
                        {
                            Direction = ParameterDirection.Input,
                            Value = newAsst
                        });

                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
