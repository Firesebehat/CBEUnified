using System;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Configuration;
using eTax.Model;

namespace CUSTOR.DataAccess
{
    public class ozekimessageoutDataAccess
    {



        private string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            }
        }



        public DataTable GetRecords()
        {
            SqlConnection connection = new SqlConnection(ConnectionString);

            string strGetAllRecords = @"SELECT [id],[sender],[receiver],[msg],[senttime],[receivedtime],[operator],[msgtype],[reference],[status],[errormsg] FROM [dbo].[ozekimessageout] ";

            SqlCommand command = new SqlCommand() { CommandText = strGetAllRecords, CommandType = CommandType.Text };
            DataTable dTable = new DataTable("ozekimessageout");
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            command.Connection = connection;

            try
            {
                connection.Open();
                adapter.Fill(dTable);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                throw new Exception("ozekimessageout::GetRecords::Error!" + ex.Message, ex);
            }
            finally
            {
                connection.Close();
                command.Dispose();
                adapter.Dispose();
            }
            return dTable;
        }



        public ozekimessageout GetRecord(string ID)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);

            ozekimessageout objozekimessageout = new ozekimessageout();
            string strGetRecord = @"SELECT [id],[sender],[receiver],[msg],[senttime],[receivedtime],[operator],[msgtype],[reference],[status],[errormsg] FROM [dbo].[ozekimessageout] ";

            SqlCommand command = new SqlCommand() { CommandText = strGetRecord, CommandType = CommandType.Text };
            DataTable dTable = new DataTable("ozekimessageout");
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            command.Connection = connection;

            try
            {
                connection.Open();
                adapter.Fill(dTable);
                if (dTable.Rows.Count > 0)
                {
                    objozekimessageout.Id = (int)dTable.Rows[0]["id"];
                    if (dTable.Rows[0]["sender"].Equals(DBNull.Value))
                        objozekimessageout.Sender = null;
                    else
                        objozekimessageout.Sender = (string)dTable.Rows[0]["sender"];
                    if (dTable.Rows[0]["receiver"].Equals(DBNull.Value))
                        objozekimessageout.Receiver = null;
                    else
                        objozekimessageout.Receiver = (string)dTable.Rows[0]["receiver"];
                    if (dTable.Rows[0]["msg"].Equals(DBNull.Value))
                        objozekimessageout.Msg = string.Empty;
                    else
                        objozekimessageout.Msg = (string)dTable.Rows[0]["msg"];
                    if (dTable.Rows[0]["senttime"].Equals(DBNull.Value))
                        objozekimessageout.Senttime = null;
                    else
                        objozekimessageout.Senttime = (string)dTable.Rows[0]["senttime"];
                    if (dTable.Rows[0]["receivedtime"].Equals(DBNull.Value))
                        objozekimessageout.Receivedtime = null;
                    else
                        objozekimessageout.Receivedtime = (string)dTable.Rows[0]["receivedtime"];
                    if (dTable.Rows[0]["operator"].Equals(DBNull.Value))
                        objozekimessageout.Operator = null;
                    else
                        objozekimessageout.Operator = (string)dTable.Rows[0]["operator"];
                    if (dTable.Rows[0]["msgtype"].Equals(DBNull.Value))
                        objozekimessageout.Msgtype = null;
                    else
                        objozekimessageout.Msgtype = (string)dTable.Rows[0]["msgtype"];
                    if (dTable.Rows[0]["reference"].Equals(DBNull.Value))
                        objozekimessageout.Reference = null;
                    else
                        objozekimessageout.Reference = (string)dTable.Rows[0]["reference"];
                    if (dTable.Rows[0]["status"].Equals(DBNull.Value))
                        objozekimessageout.Status = null;
                    else
                        objozekimessageout.Status = (string)dTable.Rows[0]["status"];
                    if (dTable.Rows[0]["errormsg"].Equals(DBNull.Value))
                        objozekimessageout.Errormsg = null;
                    else
                        objozekimessageout.Errormsg = (string)dTable.Rows[0]["errormsg"];
                }

            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                throw new Exception("ozekimessageout::GetRecord::Error!" + ex.Message, ex);
            }
            finally
            {
                connection.Close();
                command.Dispose();
                adapter.Dispose();
            }
            return objozekimessageout;
        }

        public bool Insert(ozekimessageout objozekimessageout)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);

            string strInsert = @"INSERT INTO [dbo].[ozekimessageout]
                                            ([sender],[receiver],[msg],[senttime],[receivedtime],[operator],[msgtype],[reference],[status],[errormsg])
                                     VALUES    (@sender,@receiver,@msg,@senttime,@receivedtime,@operator,@msgtype,@reference,@status,@errormsg)";

            SqlCommand command = new SqlCommand() { CommandText = strInsert, CommandType = CommandType.Text };
            command.Connection = connection;

            try
            {
                command.Parameters.Add(new SqlParameter("@sender", objozekimessageout.Sender));
                command.Parameters.Add(new SqlParameter("@receiver", objozekimessageout.Receiver));
                command.Parameters.Add(new SqlParameter("@msg", objozekimessageout.Msg));
                command.Parameters.Add(new SqlParameter("@senttime", objozekimessageout.Senttime));
                command.Parameters.Add(new SqlParameter("@receivedtime", objozekimessageout.Receivedtime));
                command.Parameters.Add(new SqlParameter("@operator", objozekimessageout.Operator));
                command.Parameters.Add(new SqlParameter("@msgtype", objozekimessageout.Msgtype));
                command.Parameters.Add(new SqlParameter("@reference", objozekimessageout.Reference));
                command.Parameters.Add(new SqlParameter("@status", objozekimessageout.Status));
                command.Parameters.Add(new SqlParameter("@errormsg", objozekimessageout.Errormsg));
                command.Parameters.Add(new SqlParameter("@id", objozekimessageout.Id));
                command.Parameters["@id"].Direction = ParameterDirection.Output;


                connection.Open();
                int _rowsAffected = command.ExecuteNonQuery();
                //objozekimessageout.Id = (int)command.Parameters["@id"].Value;
                return true;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                throw new Exception("ozekimessageout::Insert::Error!" + ex.Message, ex);
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
        }
        public bool Insert(ozekimessageout objozekimessageout, SqlConnection connection)
        {
            //SqlConnection connection = new SqlConnection(ConnectionString);

            string strInsert = @"INSERT INTO [dbo].[ozekimessageout]
                                            ([sender],[receiver],[msg],[senttime],[receivedtime],[operator],[msgtype],[reference],[status],[errormsg])
                                     VALUES    (@sender,@receiver,@msg,@senttime,@receivedtime,@operator,@msgtype,@reference,@status,@errormsg)";

            SqlCommand command = new SqlCommand() { CommandText = strInsert, CommandType = CommandType.Text };
            command.Connection = connection;

            try
            {
                command.Parameters.Add(new SqlParameter("@sender", objozekimessageout.Sender));
                command.Parameters.Add(new SqlParameter("@receiver", objozekimessageout.Receiver));
                command.Parameters.Add(new SqlParameter("@msg", objozekimessageout.Msg));
                command.Parameters.Add(new SqlParameter("@senttime", objozekimessageout.Senttime));
                command.Parameters.Add(new SqlParameter("@receivedtime", objozekimessageout.Receivedtime));
                command.Parameters.Add(new SqlParameter("@operator", objozekimessageout.Operator));
                command.Parameters.Add(new SqlParameter("@msgtype", objozekimessageout.Msgtype));
                command.Parameters.Add(new SqlParameter("@reference", objozekimessageout.Reference));
                command.Parameters.Add(new SqlParameter("@status", objozekimessageout.Status));
                command.Parameters.Add(new SqlParameter("@errormsg", objozekimessageout.Errormsg));
                command.Parameters.Add(new SqlParameter("@id", objozekimessageout.Id));
                command.Parameters["@id"].Direction = ParameterDirection.Output;


                //connection.Open();
                int _rowsAffected = command.ExecuteNonQuery();
                //objozekimessageout.Id = (int)command.Parameters["@id"].Value;
                return true;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                throw new Exception("ozekimessageout::Insert::Error!" + ex.Message, ex);
            }
            finally
            {

                command.Dispose();
            }
        }

        public bool Update(ozekimessageout objozekimessageout)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);

            string strUpdate = @"UPDATE [dbo].[ozekimessageout] SET     [sender]=@sender,    [receiver]=@receiver,    [msg]=@msg,    [senttime]=@senttime,    [receivedtime]=@receivedtime,    [operator]=@operator,    [msgtype]=@msgtype,    [reference]=@reference,    [status]=@status,    [errormsg]=@errormsg ";

            SqlCommand command = new SqlCommand() { CommandText = strUpdate, CommandType = CommandType.Text };
            command.Connection = connection;

            try
            {
                command.Parameters.Add(new SqlParameter("@id", objozekimessageout.Id));
                command.Parameters.Add(new SqlParameter("@sender", objozekimessageout.Sender));
                command.Parameters.Add(new SqlParameter("@receiver", objozekimessageout.Receiver));
                command.Parameters.Add(new SqlParameter("@msg", objozekimessageout.Msg));
                command.Parameters.Add(new SqlParameter("@senttime", objozekimessageout.Senttime));
                command.Parameters.Add(new SqlParameter("@receivedtime", objozekimessageout.Receivedtime));
                command.Parameters.Add(new SqlParameter("@operator", objozekimessageout.Operator));
                command.Parameters.Add(new SqlParameter("@msgtype", objozekimessageout.Msgtype));
                command.Parameters.Add(new SqlParameter("@reference", objozekimessageout.Reference));
                command.Parameters.Add(new SqlParameter("@status", objozekimessageout.Status));
                command.Parameters.Add(new SqlParameter("@errormsg", objozekimessageout.Errormsg));


                connection.Open();
                int _rowsAffected = command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                throw new Exception("ozekimessageout::Update::Error!" + ex.Message, ex);
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
        }

        public bool Delete(string ID)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);

            string strDelete = @"DELETE FROM [dbo].[ozekimessageout] ";

            SqlCommand command = new SqlCommand() { CommandText = strDelete, CommandType = CommandType.Text };
            command.Connection = connection;

            try
            {


                connection.Open();
                int _rowsAffected = command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                throw new Exception("ozekimessageout::Delete::Error!" + ex.Message, ex);
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
        }



        public List<ozekimessageout> GetList()
        {
            List<ozekimessageout> RecordsList = new List<ozekimessageout>();
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlDataReader dr = null;

            string strGetAllRecords = @"SELECT [id],[sender],[receiver],[msg],[senttime],[receivedtime],[operator],[msgtype],[reference],[status],[errormsg] FROM [dbo].[ozekimessageout] ";

            SqlCommand command = new SqlCommand() { CommandText = strGetAllRecords, CommandType = CommandType.Text };
            command.Connection = connection;

            try
            {
                connection.Open();
                dr = command.ExecuteReader();
                while (dr.Read())
                {
                    ozekimessageout objozekimessageout = new ozekimessageout();
                    if (dr["id"].Equals(DBNull.Value))
                        objozekimessageout.Id = 0;
                    else
                        objozekimessageout.Id = (int)dr["id"];
                    if (dr["sender"].Equals(DBNull.Value))
                        objozekimessageout.Sender = null;
                    else
                        objozekimessageout.Sender = (string)dr["sender"];
                    if (dr["receiver"].Equals(DBNull.Value))
                        objozekimessageout.Receiver = null;
                    else
                        objozekimessageout.Receiver = (string)dr["receiver"];
                    if (dr["msg"].Equals(DBNull.Value))
                        objozekimessageout.Msg = string.Empty;
                    else
                        objozekimessageout.Msg = (string)dr["msg"];
                    if (dr["senttime"].Equals(DBNull.Value))
                        objozekimessageout.Senttime = null;
                    else
                        objozekimessageout.Senttime = (string)dr["senttime"];
                    if (dr["receivedtime"].Equals(DBNull.Value))
                        objozekimessageout.Receivedtime = null;
                    else
                        objozekimessageout.Receivedtime = (string)dr["receivedtime"];
                    if (dr["operator"].Equals(DBNull.Value))
                        objozekimessageout.Operator = null;
                    else
                        objozekimessageout.Operator = (string)dr["operator"];
                    if (dr["msgtype"].Equals(DBNull.Value))
                        objozekimessageout.Msgtype = null;
                    else
                        objozekimessageout.Msgtype = (string)dr["msgtype"];
                    if (dr["reference"].Equals(DBNull.Value))
                        objozekimessageout.Reference = null;
                    else
                        objozekimessageout.Reference = (string)dr["reference"];
                    if (dr["status"].Equals(DBNull.Value))
                        objozekimessageout.Status = null;
                    else
                        objozekimessageout.Status = (string)dr["status"];
                    if (dr["errormsg"].Equals(DBNull.Value))
                        objozekimessageout.Errormsg = null;
                    else
                        objozekimessageout.Errormsg = (string)dr["errormsg"];
                    RecordsList.Add(objozekimessageout);
                }

            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                throw new Exception("ozekimessageout::GetList::Error!" + ex.Message, ex);
            }
            finally
            {
                connection.Close();
                dr.Close();
                command.Dispose();
            }
            return RecordsList;
        }


        public bool Exists(string ID)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);

            string strExists = @"SELECT  FROM [dbo].[ozekimessageout] ";

            SqlCommand command = new SqlCommand() { CommandText = strExists, CommandType = CommandType.Text };
            command.Connection = connection;

            try
            {
                return ((int)command.ExecuteScalar() > 0);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                throw new Exception("ozekimessageout::Exists::Error!" + ex.Message, ex);
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
        }


    }
}