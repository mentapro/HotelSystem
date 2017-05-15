using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Windows;

namespace HotelSystem
{
    class ADO_Lab8
    {
        private string connectionString;

        private void ExecuteSimple()
        {
            FillGrid(c => c.CommandText = "SELECT * FROM Rooms");
        }

        private void ExecuteStoredProc()
        {
            FillGrid(c =>
            {
                c.CommandType = CommandType.StoredProcedure;
                c.CommandText = "GetCurrentParam";

                AddParametersToCommand(c, new Dictionary<string, object>
                {
                    ["roomNumber"] = 5
                });
            });
        }

        private void ExecuteParametrizedSql()
        {
            FillGrid(c =>
            {
                c.CommandText = "SELECT * FROM Rooms WHERE Number = @roomNumber";

                AddParametersToCommand(c, new Dictionary<string, object>
                {
                    ["roomNumber"] = 5
                });
            });
        }

        private void AddParametersToCommand(SqlCommand c, Dictionary<string, object> dict)
        {
            foreach (var kv in dict)
            {
                c.Parameters.AddWithValue(kv.Key, kv.Value);
            }
        }

        private void FillGrid(Action<SqlCommand> commandOverride = null)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (var query = new SqlCommand()
                    {
                        Connection = conn
                    })
                    {
                        commandOverride?.Invoke(query);
                        using (DbDataReader rdr = query.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(rdr);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
