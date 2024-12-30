using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace eTaxAPI.DataAccess
{
    public class COrderDetail
    {
        private readonly string _ConnectionString = string.Empty;

        public COrderDetail()
        {
            _ConnectionString = ConnectionString;
        }

        private string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString; }
        }    
        
        public bool Insert(SqlConnection connection)
        {           
            var command = new SqlCommand
            {
                CommandText = @" INSERT [dbo].[tblOrderDetail]
                                    ([OrderGuid], [Description],[AccountCode],[Amount], [NextService],[OrderID], [Interst],[PaymentReason], [Reference], [TicketNo], [IsPenalty] )
                                VALUES
                                    (@OrderGuid, @Description, @AccountCode, @Amount, @NextService, @OrderID, @Interst, @PaymentReason, @Reference,  @TicketNo,  @IsPenalty ) ",
                CommandType = CommandType.Text,
                Connection = connection
            };
            try
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@OrderGuid", OrderGuid));
                command.Parameters.Add(new SqlParameter("@Description", Description));
                command.Parameters.Add(new SqlParameter("@AccountCode", AccountCode));
                command.Parameters.Add(new SqlParameter("@Amount", Amount));
                command.Parameters.Add(new SqlParameter("@NextService", NextService));
                command.Parameters.Add(new SqlParameter("@Id", Id));
                command.Parameters.Add(new SqlParameter("@OrderID", OrderID));
                command.Parameters.Add(new SqlParameter("@Interst", Interst));
                command.Parameters.Add(new SqlParameter("@PaymentReason", PaymentReason));
                command.Parameters.Add(new SqlParameter("@Reference", Reference));
                command.Parameters.Add(new SqlParameter("@TicketNo", TicketNo));
                command.Parameters.Add(new SqlParameter("@IsPenalty", IsPenalty));
                //command.Parameters["@Id"].Direction = ParameterDirection.Output;              
                var _rowsAffected = command.ExecuteNonQuery();
                Id = (int) command.Parameters["@Id"].Value;
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {             
                command.Dispose();
            }
        }

        public bool Delete(Guid OrderGUID, SqlConnection connection)
        {
            var command = new SqlCommand();
            command.CommandText = "DELETE FROM [dbo].[tblOrderDetail] WHERE [OrderGUID] = @OrderGUID";
            command.CommandType = CommandType.Text;
            command.Connection = connection;
            try
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@OrderGUID", OrderGUID));
                var _rowsAffected = command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("CtblOrderDetail::Delete::Error!" + ex.Message, ex);
            }
            finally
            {
                command.Dispose();
            }
        }

        public bool Update(SqlConnection connection)
        {

            var command = new SqlCommand
            {
                CommandText = @"UPDATE [dbo].[tblOrderDetail] SET
                                       ([Amount] = @Amount)
                                WHERE (OrderGuid = @OrderGUID",
                CommandType = CommandType.Text,
                Connection = connection
            };
            try
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@Amount", Amount));
                command.Parameters.Add(new SqlParameter("@OrderGUID", OrderGuid));

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                command.Dispose();
            }
        }

        #region Class Property Declarations

        public int Id { get; set; }

        private Guid _main_guid;

        public Guid main_guid
        {
            get { return main_guid; }
            set { main_guid = value; }
        }

        public Guid OrderGuid { get; set; }

        public string Description { get; set; }

        public string AmDescription { get; set; }

        public string AccountCode { get; set; }

        public decimal Amount { get; set; }

        public int NextService { get; set; }

        public int OrderID { get; set; }


        public decimal Interst { get; set; }

        public int PaymentReason { get; set; }

        public int Reference { get; set; }

        public string TicketNo { get; set; }


        public bool IsPenalty { get; set; }

        #endregion
    }
}