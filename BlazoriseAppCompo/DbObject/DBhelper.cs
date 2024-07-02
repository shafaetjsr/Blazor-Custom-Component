using Microsoft.Data.SqlClient;
using System.Data.Common;
using System.Data;
using BlazoriseAppCompo.Data;
using Newtonsoft.Json;

namespace GGAPost.Data.DbObject
{
    public class DBhelper
    {

        public DbCommand CreateCommand(string SQLQuery, CommandType commandType)
        {
            SqlConnection obcon = new SqlConnection(DbConnection.CName);
            DbCommand cmd = obcon.CreateCommand();
            cmd.CommandText = SQLQuery;
            cmd.CommandType = commandType;
            return cmd;

        }

        public DbCommand ExecuteSP(string SqlQuery, CommandType commandType)
        {
            return CreateCommand(SqlQuery, commandType);
        }

        public DbParameter CreateParameter(string name, object value)
        {
            DbParameter param = DBprovider.Create().CreateParameter();
            param.Value = value;
            param.ParameterName = name;
            return param;
        }
        public DbParameter CreateParameterOut(string name, DbType dbtype, int size)
        {
            DbParameter param = DBprovider.Create().CreateParameter();
            param.Size = size;
            param.DbType = dbtype;
            param.ParameterName = name;
            param.Direction = ParameterDirection.Output;
            return param;
        }

        public DataTable ExecuteSQLDatatable(string SQLQuery)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection obcon = new SqlConnection(DbConnection.CName);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = obcon;
                SqlCommand cmd = da.SelectCommand;
                cmd.CommandText = SQLQuery;
                cmd.CommandType = CommandType.Text;                
                da.Fill(dt);
                
            }
            catch (Exception ex)
            {
                dt = new DataTable();
                dt.Columns.Add("vCode");
                dt.Columns.Add("vMsg");
                DataRow dr = dt.NewRow();
                dr["vCode"] = "0";
                dr["vMsg"] = ex.Message;
                dt.Rows.Add(dr);
            }
            return dt;
        }
        public DbCommand ExecutePlanSQL(string sqlQuery)
        {
            return CreateCommand(sqlQuery, CommandType.Text);
        }
        public int ExecuteCommand(string command)
        {
            string query = command.ToUpper();
            if (query.Contains("DROP") || query.Contains("TRUNCATE"))
            {
                return 0;
            }
            DbCommand cmd = ExecutePlanSQL(command);
            cmd.Connection.Open();
            cmd.CommandTimeout = 9000;
            int result = 0;
            try
            {

                result = cmd.ExecuteNonQuery();
                return result;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
        }

        public DataTable ExecuteSQLDatatableWithCon(string SQLQuery, SqlConnection obcon)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = obcon;
                SqlCommand cmd = da.SelectCommand;
                cmd.CommandText = SQLQuery;
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                dt = getRvDT("0", ex.Message);
            }
            return dt;

        }

        public DataTable getRvDT(string vCode, string vMsg)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("vCode");
            dt.Columns.Add("vMsg");

            DataRow dr = dt.NewRow();
            dr[0] = vCode;
            dr[1] = vMsg;
            dt.Rows.Add(dr);

            return dt;
        }

        public async Task<DataTable> getAutocompleteData(string sqlQuery, string searchKey)
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(DbConnection.CName))
            {
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    // Add the parameter and set its value
                    command.Parameters.AddWithValue("@searchKey", "%" + searchKey + "%");

                    // Open the connection
                    await connection.OpenAsync();

                    // Execute the query and load the results into a DataTable
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            return dt;
        }
        
    }
}
