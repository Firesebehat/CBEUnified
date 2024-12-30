using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace eTaxAPI.DataAccess
{
    public class TelephoneDataAccess
    {
        private string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString; }
        }

        public bool IsIMEINumberAllowed(string IMEINumber)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            string strGetRecord = @"SELECT [PhoneNumber],[IMEINumber],[UserName] FROM [dbo].[aspnet_PhoneInfo] WHERE [IMEINumber]=@IMEINumber";
            SqlCommand command = new SqlCommand() { CommandText = strGetRecord, CommandType = CommandType.Text };
            DataTable dTable = new DataTable("Telphone");
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            command.Connection = connection;
            try
            {
                command.Parameters.Add(new SqlParameter("@IMEINumber", IMEINumber));
                connection.Open();
                adapter.Fill(dTable);
                if (dTable.Rows.Count > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Telphone::IsIMEINumberAllowed::Error!" + ex.Message, ex);
            }
            finally
            {
                connection.Close();
                command.Dispose();
                adapter.Dispose();
            }
            return false;
        }
    }
}