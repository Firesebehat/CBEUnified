using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using eTaxAPI.Model;
using System.Transactions;

namespace eTaxAPI.DataAccess
{
    public class CPayment
    {
        private static string ConnectionString
            => ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        public DateTime GetTodayDate()
        {
            var connection = new SqlConnection(ConnectionString);
            const string strGetRecord = @"SELECT GetDate()";
            var command = new SqlCommand
            {
                CommandText = strGetRecord,
                CommandType = CommandType.Text,
                Connection = connection
            };
            try
            {
                connection.Open();
                var dt = (DateTime)command.ExecuteScalar();
                return dt;
            }
            catch (Exception)
            {
                return DateTime.Now;
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
        }

    }
}