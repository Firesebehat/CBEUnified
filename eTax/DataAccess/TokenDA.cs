using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using eTaxAPI.Models;

namespace eTaxAPI.DataAccess
{
    public class TokenDA
    {
        private string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString; }
        }

        public bool Insert(Token objToken)
        {
            var connection = new SqlConnection(ConnectionString);

            const string strInsert = @"INSERT INTO [dbo].[Tokens]
                                                   (UserName,[AuthToken]
                                                   ,[IssuedOn]
                                                   ,[ExpiresOn])
                                        VALUES
                                               (@UserName,
                                                @AuthToken, 
                                                @IssuedOn,
                                                @ExpiresOn)";

            var command = new SqlCommand
            {
                CommandText = strInsert,
                CommandType = CommandType.Text,
                Connection = connection
            };
            try
            {
                command.Parameters.Add(new SqlParameter("@AuthToken", objToken.AuthToken));
                command.Parameters.Add(new SqlParameter("@IssuedOn", objToken.IssuedOn));
                command.Parameters.Add(new SqlParameter("@ExpiresOn", objToken.ExpiresOn));
                command.Parameters.Add(new SqlParameter("@UserName", objToken.UserName));
                command.Parameters.Add(new SqlParameter("@TokenId", objToken.TokenId));
                command.Parameters["@TokenId"].Direction = ParameterDirection.Output;


                connection.Open();
                var _rowsAffected = command.ExecuteNonQuery();
                //objToken.TokenId = (int)command.Parameters["@TokenId"].Value;
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("TokenDA::Insert::Error!" + ex.Message, ex);
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
        }

        public Token GetRecord(string ID)
        {
            var connection = new SqlConnection(ConnectionString);

            var objTokens = new Token();
            const string strGetRecord = @"SELECT [TokenId],[UserName],[AuthToken],[IssuedOn],[ExpiresOn] 
                                        FROM [dbo].[Tokens] 
                                        WHERE [AuthToken]=@AuthToken and ExpiresOn>GetDate()";

            var command = new SqlCommand {CommandText = strGetRecord, CommandType = CommandType.Text};
            var dTable = new DataTable("Tokens");
            var adapter = new SqlDataAdapter(command);
            command.Connection = connection;

            try
            {
                command.Parameters.Add(new SqlParameter("@AuthToken", ID));
                connection.Open();
                adapter.Fill(dTable);
                if (dTable.Rows.Count > 0)
                {
                    objTokens.TokenId = dTable.Rows[0]["TokenId"].Equals(DBNull.Value)
                        ? 0
                        : (int) dTable.Rows[0]["TokenId"];
                    objTokens.UserName = dTable.Rows[0]["UserName"].Equals(DBNull.Value)
                        ? string.Empty
                        : (string) dTable.Rows[0]["UserName"];
                    objTokens.AuthToken = dTable.Rows[0]["AuthToken"].Equals(DBNull.Value)
                        ? string.Empty
                        : dTable.Rows[0]["AuthToken"].ToString();
                    objTokens.IssuedOn = dTable.Rows[0]["IssuedOn"].Equals(DBNull.Value)
                        ? DateTime.MinValue
                        : (DateTime) dTable.Rows[0]["IssuedOn"];
                    objTokens.ExpiresOn = dTable.Rows[0]["ExpiresOn"].Equals(DBNull.Value)
                        ? DateTime.MinValue
                        : (DateTime) dTable.Rows[0]["ExpiresOn"];
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Tokens::GetRecord::Error!" + ex.Message, ex);
            }
            finally
            {
                connection.Close();
                command.Dispose();
                adapter.Dispose();
            }
            return objTokens;
        }

        public bool Update(string strAuthToken, DateTime dtExpiresOn)
        {
            var connection = new SqlConnection(ConnectionString);

            const string strUpdate = @"UPDATE [dbo].[Tokens] SET  [ExpiresOn]=@ExpiresOn WHERE [AuthToken]=@AuthToken";

            var command = new SqlCommand
            {
                CommandText = strUpdate,
                CommandType = CommandType.Text,
                Connection = connection
            };

            try
            {
                command.Parameters.Add(new SqlParameter("@AuthToken", strAuthToken));
                command.Parameters.Add(new SqlParameter("@ExpiresOn", dtExpiresOn));

                connection.Open();
                var rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Tokens::Update::Error!" + ex.Message, ex);
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
        }

        public bool Delete(string ID)
        {
            var connection = new SqlConnection(ConnectionString);

            const string strDelete = @"DELETE FROM [dbo].[Tokens] WHERE [AuthToken]=@AuthToken";

            var command = new SqlCommand
            {
                CommandText = strDelete,
                CommandType = CommandType.Text,
                Connection = connection
            };

            try
            {
                command.Parameters.Add(new SqlParameter("@AuthToken", ID));


                connection.Open();
                var rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Tokens::Delete::Error!" + ex.Message, ex);
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
        }
    }
}