using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using eTaxAPI.Model;
using System.Transactions;
using eTaxAPI.Repository;
using eTax.Model;
using CUSTOR.Bussiness;

namespace eTaxAPI.DataAccess
{
    public class CtblOrder
    {
        private readonly string _ConnectionString = string.Empty;

        public CtblOrder()
        {
            _ConnectionString = ConnectionString;
        }
        private string ConnectionString => ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        public bool UpdateEx(Orders objOrders)
        {
            SqlConnection connection = new SqlConnection(_ConnectionString);
            SqlCommand command = new SqlCommand();
            SqlTransaction Tran;
            command.Connection = connection;
            connection.Open();
            Tran = connection.BeginTransaction();
            command.Transaction = Tran;
            try
            {
                bool isOrderUpdated = UpdateOrder(objOrders, connection, command);
                if (isOrderUpdated)
                {
                    if (objOrders.TellNo.Length >= 10)
                        InsertMessage(connection, command, objOrders);
                    Tran.Commit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
            return false;
        }
        public bool UpdateDarsEx(Orders objOrders)
        {

            SqlConnection connection = new SqlConnection(ConnectionString);
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    using (connection)
                    {
                        connection.Open();
                        bool isOrderUpdated = UpdateOrderDars(objOrders, connection);
                        if (isOrderUpdated)
                        {
                            ts.Complete();
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                connection.Close();
            }
        }

        public bool UpdateOrderDars(Orders objOrders, SqlConnection connection)
        {
            ozekimessageout objozekimessageout = new ozekimessageout();
            ozekimessageoutBussiness objozekimessageoutBussiness = new ozekimessageoutBussiness();
            var orderResult = false;
            var strSQL = @"UPDATE [dbo].[Order]
                        SET 
	                        [ReceiptNo] = @ReceiptNo, [PaymentDate] = @PaymentDate, [IsPaid] = @Paid, [CheckNo] = @CheckNo, 
                            [UpdatedUsername] = @UpdatedUsername, BankCode=@BankCode, BankName=@BankName, PaymentMethod=@PaymentMethod,ModeOfCollection=@ModeOfCollection,IsVoid=@IsVoid
                        WHERE
	                        [OrderId] = @OrderId";
            var command = new SqlCommand
            {
                CommandText = strSQL,
                CommandType = CommandType.Text,
                Connection = connection
            };
            try
            {
                command.Parameters.Add(new SqlParameter("@ReceiptNo", objOrders.ReceiptNo));
                command.Parameters.Add(new SqlParameter("@CheckNo", objOrders.CheckNo));
                command.Parameters.Add(new SqlParameter("@Paid", objOrders.IsPaid));
                command.Parameters.Add(new SqlParameter("@IsVoid", objOrders.IsVoid));
                command.Parameters.Add(new SqlParameter("@UserName", objOrders.UserName));
                command.Parameters.Add(new SqlParameter("@UpdatedUsername", objOrders.UpdatedUsername));
                command.Parameters.Add(new SqlParameter("@OrderId", objOrders.OrderId));
                command.Parameters.Add(new SqlParameter("@PaymentDate", objOrders.PaymentDate));
                command.Parameters.Add(new SqlParameter("@BankCode", objOrders.BankCode));
                command.Parameters.Add(new SqlParameter("@BankName", objOrders.BankName));
                command.Parameters.Add(new SqlParameter("@PaymentMethod", objOrders.PaymentMethod));
                command.Parameters.Add(new SqlParameter("@ModeOfCollection", objOrders.ModeOfCollection));

                //command.ExecuteNonQuery();
                //if (objOrders.Parent_guid != Guid.Empty.ToString())
                //AssiginNextStep(connection, objOrders.ServiceGuid, new Guid(objOrders.Parent_guid));
                orderResult = command.ExecuteNonQuery() >= 1;
                if (orderResult)
                {
                    string Msg = "ውድ ተገልጋያችን " + objOrders.ServiceName + " ክፍያ ብር " + objOrders.Amount + " በደረሰኝ ቁጥር " + objOrders.ReceiptNo + " ተከፍሏል የሰነዶች ማረጋገጫና ምዝገባ አገልግሎት";
                    string MobileNo = objOrders.MobileNo;
                    DoSaveMessage(Msg, MobileNo, connection);
                }
                return true;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                throw new Exception("Customer::Update::Error!" + ex.Message, ex);
            }
            finally
            {
                command.Dispose();
            }
        }
        protected bool DoSaveMessage(string Msg, string MobileNo, SqlConnection connection)
        {

            //string Msg = "የ ሰነዶች ማረጋገጫ ና ምዝገባ አገልግሎት " + objOrders.ServiceName + " ክፍያ ብር " + objOrders.Amount + " በደረሰኝ ቁጥር " + objOrders.ReceiptNo + " ተከፍሏል";
            ozekimessageout objozekimessageout = new ozekimessageout();
            try
            {
                //objozekimessageout.Id = 1;// Use Identity Number
                objozekimessageout.Sender = "9494";
                objozekimessageout.Receiver = MobileNo;
                objozekimessageout.Msg = Msg;
                objozekimessageout.Senttime = DateTime.Now.ToLongDateString();
                objozekimessageout.Receivedtime = string.Empty;
                objozekimessageout.Operator = string.Empty;
                objozekimessageout.Msgtype = string.Empty;
                objozekimessageout.Reference = string.Empty;
                objozekimessageout.Status = "send";
                objozekimessageout.Errormsg = string.Empty;


                ozekimessageoutBussiness objozekimessageoutBussiness = new ozekimessageoutBussiness();
                objozekimessageoutBussiness.Insertozekimessageout(objozekimessageout, connection);

                //else
                //{
                //    objozekimessageout.Id= 0;//Attention
                //    objozekimessageoutBussiness.Updateozekimessageout(objozekimessageout);
                //}

                string s = (" Record was saved successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                string z = (ex.Message);
                return false;
            }
        }
        private void InsertMessage(SqlConnection connection, SqlCommand command, Orders objOrders)
        {
            string msg = string.Empty;
            string strInsert = @"INSERT INTO ozekimessageout
                                             ([sender],[receiver],[msg],[senttime],[status])
                                     VALUES  (@sender, @receiver, @msg, @senttime, @status) ";
            command.CommandText = strInsert;
            command.CommandType = CommandType.Text;
            command.Parameters.Clear();
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["DireDawaEnabled"]))
            {
                msg = @"ውድ ግብር ከፋያችን (ቲን " + objOrders.Tin + ") ብር " + objOrders.TotalPayment + " ግብርዎን በመክፈል ግዴታዎን ስለተወጡ እናመሰግናለን፡፡ የድሬዳዋ ከተማ አስተዳደር ገቢዎች ባለስልጣን";
                command.Parameters.Add(new SqlParameter("@sender", "የድሬዳዋ ከተማ አስተዳደር ገቢዎች ባለስልጣን"));
            }
            else if (Convert.ToBoolean(ConfigurationManager.AppSettings["SomaliEnabled"]))
            {
                msg = @"ውድ ግብር ከፋያችን (ቲን " + objOrders.Tin + ") ብር " + objOrders.TotalPayment + " ግብርዎን በመክፈል ግዴታዎን ስለተወጡ እናመሰግናለን፡፡ የሱማሌ ክልል ገቢዎች ቢሮ";
                command.Parameters.Add(new SqlParameter("@sender", "የሱማሌ ክልል ገቢዎች ቢሮ"));
            }
            else if (Convert.ToBoolean(ConfigurationManager.AppSettings["DarsEnabled"]))
            {
                string Msg = "ውድ ተገልጋያችን " + objOrders.ServiceID + " ክፍያ ብር " + objOrders.TotalPayment + " በደረሰኝ ቁጥር " + objOrders.ReceiptNo + " ተከፍሏል የሰነዶች ማረጋገጫና ምዝገባ አገልግሎት";
                command.Parameters.Add(new SqlParameter("@sender", "የፌደራል ሰነዶች ማረጋገጫና ምዝገባ አገልግሎት"));
            }
            else if (Convert.ToBoolean(ConfigurationManager.AppSettings["MotriEnabled"]))
            {
                string Msg = "ውድ ተገልጋያችን " + objOrders.ServiceID + " ክፍያ ብር " + objOrders.TotalPayment + " በደረሰኝ ቁጥር " + objOrders.ReceiptNo + " ተከፍሏል የንግድና ቀጠናዊ ትስስር ሚኒስትር";
                command.Parameters.Add(new SqlParameter("@sender", "ንግድና ቀጠናዊ ትስስር ሚኒስትር"));
            }
            else
            {
                msg = @"ውድ ግብር ከፋያችን (ቲን " + objOrders.Tin + ") ብር " + objOrders.TotalPayment + " ግብርዎን በመክፈል ግዴታዎን ስለተወጡ እናመሰግናለን፡፡ የአዲስ አበባ ገቢዎች ቢሮ";
                command.Parameters.Add(new SqlParameter("@sender", "የአዲስ አበባ ገቢዎች ቢሮ"));
            }
            //string msg = @"ውድ ግብር ከፋያችን (ቲን " + objOrders.Tin + ") ብር " + objOrders.TotalPayment + " የ2015 ዓመታዊ ግብርዎን በመክፈል ግዴታዎን ስለተወጡ እናመሰግናለን፡፡ የአዲስ አበባ ገቢዎች ቢሮ";
            command.Parameters.Add(new SqlParameter("@receiver", objOrders.TellNo));
            command.Parameters.Add(new SqlParameter("@msg", msg));
            command.Parameters.Add(new SqlParameter("@senttime", DateTime.Now));
            command.Parameters.Add(new SqlParameter("@status", "send"));
            int _rowsAffected = command.ExecuteNonQuery();
        }
        public bool UpdateOrder(Orders objOrders, SqlConnection connection, SqlCommand command)
        {
            //--, [CheckNo] = @CheckNo, 
            var strSQL = @"UPDATE Payment
                                SET 
	                                [ReceiptNo] = @ReceiptNo, DatePaid = @PaymentDate, [IsPaid] = @Paid,
                                    [CashierName] = @CashierUserName, BankCode=@BankCode, BankName=@BankName, PaymentMethod=@PaymentMethod
                                WHERE
	                                [Id] = @InvoiceNo";
            command.CommandText = strSQL;
            command.CommandType = CommandType.Text;
            command.Connection = connection;
            try
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@ReceiptNo", objOrders.ReceiptNo));
                //command.Parameters.Add(new SqlParameter("@CheckNo", objOrders.CheckNo));
                command.Parameters.Add(new SqlParameter("@Paid", objOrders.IsPaid));
                command.Parameters.Add(new SqlParameter("@CashierUserName", objOrders.CashierUserName));
                command.Parameters.Add(new SqlParameter("@InvoiceNo", objOrders.InvoiceNo));
                command.Parameters.Add(new SqlParameter("@PaymentDate", objOrders.PaymentDate));
                command.Parameters.Add(new SqlParameter("@BankCode", objOrders.BankCode));
                command.Parameters.Add(new SqlParameter("@BankName", objOrders.BankName));
                command.Parameters.Add(new SqlParameter("@PaymentMethod", objOrders.PaymentMethod));

                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Customer::Update::Error!" + ex.Message, ex);
            }
            finally
            {

            }
        }
        public bool VoidPaymentByOrderId(int InvoiceNo)
        {
            var strSQL = "UPDATE  Payment   SET [Void]=@void,  [IsPaid] = 0, VoidDate=GetDate()  where Id = @InvoiceNo ";
            try
            {
                using (var ts = new TransactionScope())
                {
                    using (var con = new SqlConnection(ConnectionString))
                    {
                        con.Open();
                        var orderResult = false;
                        using (var cmd1 = con.CreateCommand())
                        {
                            cmd1.CommandType = CommandType.Text;

                            cmd1.CommandText = strSQL;
                            cmd1.Parameters.Add(new SqlParameter("@InvoiceNo", InvoiceNo));
                            cmd1.Parameters.Add(new SqlParameter("@void", true));
                            orderResult = cmd1.ExecuteNonQuery() >= 1;
                        }
                        if (orderResult)
                        {
                            // Commit the transaction 
                            ts.Complete();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                return false;
                // the transaction scope will take care of rolling back
            }
            return true;
        }
        public bool VoidPayment(string strReceiptNo, string strInvoiceNo)
        {
            var strSQL = "UPDATE  Payment SET [void]=@void, VoidDate=GetDate() where ReceiptNo=@ReceiptNo";
            try
            {
                using (var ts = new TransactionScope())
                {
                    using (var con = new SqlConnection(ConnectionString))
                    {
                        con.Open();
                        var orderResult = false;
                        using (var cmd1 = con.CreateCommand())
                        {
                            cmd1.CommandType = CommandType.Text;
                            cmd1.CommandText = strSQL;
                            cmd1.Parameters.Add(new SqlParameter("@ReceiptNo", strReceiptNo));
                            cmd1.Parameters.Add(new SqlParameter("@void", true));
                            orderResult = cmd1.ExecuteNonQuery() >= 1;
                        }
                        if (orderResult)
                        {
                            // Commit the transaction 
                            ts.Complete();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                return false;
                // the transaction scope will take care of rolling back
            }
            return true;
        }
        public Orders GetPaymentByReceiptNo(string strReceiptNo)
        {
            var connection = new SqlConnection(_ConnectionString);
            var strSQL = @"SELECT  Id, DatePrepared,IsPaid,Void FROM Payment WHERE ReceiptNo = @ReceiptNo ";
            var command = new SqlCommand { CommandText = strSQL, CommandType = CommandType.Text };
            var dTable = new DataTable("Order");
            var adapter = new SqlDataAdapter(command);
            command.Connection = connection;
            var Ord = new Orders();
            try
            {
                command.Parameters.Add(new SqlParameter("@ReceiptNo", strReceiptNo));
                connection.Open();
                adapter.Fill(dTable);
                if (dTable.Rows.Count > 0)
                {
                    var dr = dTable.Rows[0];
                    Ord.InvoiceNo = Convert.ToInt32(dr["Id"]);
                    Ord.IsPaid = !dr["IsPaid"].Equals(DBNull.Value) && Convert.ToBoolean(dr["IsPaid"].ToString());
                    Ord.IsVoid = !dr["Void"].Equals(DBNull.Value) && Convert.ToBoolean(dr["Void"].ToString());
                    return Ord;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetPaymentByReceiptNo: " + ex.Message);
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
        }
        public Orders GetOrderByOrderNo(int InvoiceNo)
        {
            var connection = new SqlConnection(_ConnectionString);
            var strSQL = @"SELECT   Id, DatePrepared,ISPaid,Void, BankCode FROM  Payment WHERE Id = @InvoiceNo ";
            var command = new SqlCommand { CommandText = strSQL, CommandType = CommandType.Text };
            var dTable = new DataTable("Order");
            var adapter = new SqlDataAdapter(command);
            command.Connection = connection;
            var Ord = new Orders();
            try
            {
                command.Parameters.Add(new SqlParameter("@InvoiceNo", InvoiceNo));
                connection.Open();
                adapter.Fill(dTable);
                if (dTable.Rows.Count > 0)
                {
                    var dr = dTable.Rows[0];
                    Ord.InvoiceNo = Convert.ToInt32(dr["Id"]);
                    Ord.IsPaid = dr["ISPaid"].Equals(DBNull.Value) ? false : Convert.ToBoolean(dr["ISPaid"].ToString());
                    Ord.IsVoid = dr["Void"].Equals(DBNull.Value) ? false : Convert.ToBoolean(dr["Void"].ToString());
                    Ord.SiteID = dr["BankCode"].Equals(DBNull.Value) ? "" : Convert.ToString(dr["BankCode"].ToString());
                }
                return Ord;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetPaymentByReceiptNo: " + ex.Message);
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
        }
        public UnifiedPaymentReturnValue GetUnifiedOrderByBillNo(int OrderId)
        {
            var connection = new SqlConnection(_ConnectionString);
            var strSQL = @"  SELECT top(1) Payment.Id AS OrderId, Payment.DatePrepared AS OrderDate, Payment.IsPaid, Payment.Void AS IsVoid, 
									 TotalAmount =Payment.TotalPayment, DATEDIFF(DAY,Payment.DatePrepared,GETDATE()) NoOfDays,
                                 'CustomerName'=  BusinessNameAmh , 
                                 'ServiceTaken'=  'Tax Payment',
                                 'Tin'= Tin ,
                                   Payment.AccountNumber, MaximumDateBeforeRecaclulation
                     FROM 
                         Payment WHERE  Payment.ID = @OrderId and isnull(Void,0)=0 and isnull(IsPaid,0)=0 and DATEDIFF(DAY,Payment.DatePrepared,GETDATE())<30
                     Group by 
                         Payment.Id, ReceiptNo, Payment.DatePaid, CustomerId, Payment.TotalPayment,Payment.BusinessNameAmh,
						    IsPaid, Payment.Void, Payment.DatePrepared, checkno ,AccountNumber, MaximumDateBeforeRecaclulation ,Payment.Tin                      
                     order by DatePrepared desc  ";
            var command = new SqlCommand { CommandText = strSQL, CommandType = CommandType.Text };
            var dTable = new DataTable("Order");
            var adapter = new SqlDataAdapter(command);
            command.Connection = connection;

            var PaymentDetail = new UnifiedPaymentReturnValue();
            try
            {
                command.Parameters.Add(new SqlParameter("@OrderId", OrderId));
                connection.Open();
                adapter.Fill(dTable);
                if (dTable.Rows.Count > 0)
                {
                    var dr = dTable.Rows[0];
                    PaymentDetail.Bill_Id = Convert.ToInt32(dr["OrderId"]);
                    //PaymentDetail.OrderedDate = dr["OrderDate"].Equals(DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(dr["OrderDate"]);
                    PaymentDetail.Total_Amount = dr["TotalAmount"].Equals(DBNull.Value) ? 0 : Convert.ToDecimal(dr["TotalAmount"]);
                    PaymentDetail.Full_Name = dr["CustomerName"].Equals(DBNull.Value) ? String.Empty : Convert.ToString(dr["CustomerName"]);
                    PaymentDetail.Tin_Number = dr["Tin"].Equals(DBNull.Value) ? String.Empty : Convert.ToString(dr["Tin"]);
                    PaymentDetail.Credit_Acct_Number = dr["AccountNumber"].Equals(DBNull.Value) ? String.Empty : Convert.ToString(dr["AccountNumber"]);
                    PaymentDetail.Payment_Reason = dr["ServiceTaken"].Equals(DBNull.Value) ? String.Empty : Convert.ToString(dr["ServiceTaken"]);
                    //PaymentDetail.IsPaid = dr["ISPaid"].Equals(DBNull.Value) ? false : Convert.ToBoolean(dr["ISPaid"].ToString());
                    //PaymentDetail.IsVoid = dr["IsVoid"].Equals(DBNull.Value) ? false : Convert.ToBoolean(dr["IsVoid"].ToString());
                    //PaymentDetail.NoOfDays = dr["NoOfDays"].Equals(DBNull.Value) ? 0 : Convert.ToInt16(dr["NoOfDays"].ToString());
                    PaymentDetail.Destination_Api_Name = "Third Party Query";
                    PaymentDetail.Status = "Success";

                    //DateTime MaximumDateBeforeRecaclulation = dr["MaximumDateBeforeRecaclulation"].Equals(DBNull.Value) ? DateTime.MaxValue : Convert.ToDateTime(dr["MaximumDateBeforeRecaclulation"]);
                    //if (DateTime.Today > MaximumDateBeforeRecaclulation)
                    //{
                    //    RecaclulatePayment(OrderId);
                    //    GetOrderByInvoiceNo(OrderId);
                    //}
                    PaymentDetail.Response_Code = 0;
                    PaymentDetail.Response_Description = "";
                    //PaymentDetail.ErrorCode = 200;
                    //PaymentDetail.ErrorMessage = "";
                }
                else
                {
                    PaymentDetail.Response_Code = 101;
                    PaymentDetail.Response_Description = "";

                    //PaymentDetail.ErrorCode = 101;
                    //PaymentDetail.ErrorMessage = "";
                }
                return PaymentDetail;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetPaymentByReceiptNo: " + ex.Message);
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
        }
        public UnifiedPaymentReturnValue GetOrderByInvoiceNoCBE(int OrderId)
        {
            var connection = new SqlConnection(_ConnectionString);
            var strSQL = @"SELECT   top(1)  [Order].OrderID,[Order].OrderDate,[Order].IsPaid,[Order].IsVoid,Sum(isnull(OrderDetail.Amount,0.00)) as TotalAmount,
                              [Order].PaidBy as CustomerName,[Order].UserName,[Order].AccountNumberCBE,'' Tin,
	                           (SELECT top(1) ServiceNameEnglish FROM  [Service] where ServiceTypeCode= [Order].ServiceTypeCode) as ServiceTaken
                                                      From [Order]  
							                        inner join OrderDetail on [Order].OrderGuid=OrderDetail.OrderGuid							
                                                    WHERE  [Order].OrderGuid=OrderDetail.OrderGuid   AND   [Order].OrderID = @OrderId  and isnull(IsVoid,0)=0 and isnull(IsPaid,0)=0 and DATEDIFF(DAY,Order.OrderDate,GETDATE())<30
                                                    Group by [Order].OrderID,[Order].ServiceApplicationGuid,ReceiptNo,PaymentDate,[Order].ServiceTypeCode, IsPaid, IsVoid, [Order].OrderDate, checkno,[Order].PaidBy,[Order].UserName,[Order].AccountNumberCBE 
                                                    order by PaymentDate desc";
            var command = new SqlCommand { CommandText = strSQL, CommandType = CommandType.Text };
            var dTable = new DataTable("Order");
            var adapter = new SqlDataAdapter(command);
            command.Connection = connection;

            var PaymentDetail = new UnifiedPaymentReturnValue();
            try
            {
                command.Parameters.Add(new SqlParameter("@OrderId", OrderId));
                connection.Open();
                adapter.Fill(dTable);
                if (dTable.Rows.Count > 0)
                {
                    var dr = dTable.Rows[0];
                    PaymentDetail.Bill_Id = Convert.ToInt32(dr["OrderId"]);
                    //PaymentDetail.OrderedDate = dr["OrderDate"].Equals(DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(dr["OrderDate"]);
                    PaymentDetail.Total_Amount = dr["TotalAmount"].Equals(DBNull.Value) ? 0 : Convert.ToDecimal(dr["TotalAmount"]);
                    PaymentDetail.Full_Name = dr["CustomerName"].Equals(DBNull.Value) ? String.Empty : Convert.ToString(dr["CustomerName"]);
                    PaymentDetail.Tin_Number = dr["Tin"].Equals(DBNull.Value) ? String.Empty : Convert.ToString(dr["Tin"]);
                    PaymentDetail.Credit_Acct_Number = dr["AccountNumberCBE"].Equals(DBNull.Value) ? String.Empty : Convert.ToString(dr["AccountNumberCBE"]);
                    PaymentDetail.Payment_Reason = dr["ServiceTaken"].Equals(DBNull.Value) ? String.Empty : Convert.ToString(dr["ServiceTaken"]);
                    //PaymentDetail.IsPaid = dr["ISPaid"].Equals(DBNull.Value) ? false : Convert.ToBoolean(dr["ISPaid"].ToString());
                    //PaymentDetail.IsVoid = dr["IsVoid"].Equals(DBNull.Value) ? false : Convert.ToBoolean(dr["IsVoid"].ToString());
                    //PaymentDetail.NoOfDays = dr["NoOfDays"].Equals(DBNull.Value) ? 0 : Convert.ToInt16(dr["NoOfDays"].ToString());
                    PaymentDetail.Destination_Api_Name = "Third Party Query";
                    PaymentDetail.Status = "Success";

                    //DateTime MaximumDateBeforeRecaclulation = dr["MaximumDateBeforeRecaclulation"].Equals(DBNull.Value) ? DateTime.MaxValue : Convert.ToDateTime(dr["MaximumDateBeforeRecaclulation"]);
                    //if (DateTime.Today > MaximumDateBeforeRecaclulation)
                    //{
                    //    RecaclulatePayment(OrderId);
                    //    GetOrderByInvoiceNo(OrderId);
                    //}
                    PaymentDetail.Response_Code = 0;
                    PaymentDetail.Response_Description = "";
                    //PaymentDetail.ErrorCode = 200;
                    //PaymentDetail.ErrorMessage = "";
                }
                else
                {
                    PaymentDetail.Response_Code = 101;
                    PaymentDetail.Response_Description = "";

                    //PaymentDetail.ErrorCode = 101;
                    //PaymentDetail.ErrorMessage = "";
                }
                return PaymentDetail;

                //    var PaymentDetail = new PaymentReturnValue();
                //try
                //{
                //    command.Parameters.Add(new SqlParameter("@OrderId", OrderId));
                //    connection.Open();
                //    adapter.Fill(dTable);
                //    if (dTable.Rows.Count > 0)
                //    {
                //        var dr = dTable.Rows[0];
                //        PaymentDetail.OrderId = Convert.ToInt32(dr["OrderId"]);
                //        PaymentDetail.OrderedDate = dr["OrderDate"].Equals(DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(dr["OrderDate"]);
                //        PaymentDetail.TotalAmount = dr["TotalAmount"].Equals(DBNull.Value) ? 0 : Convert.ToDecimal(dr["TotalAmount"]);
                //        PaymentDetail.CustomerName = dr["CustomerName"].Equals(DBNull.Value) ? String.Empty : Convert.ToString(dr["CustomerName"]);
                //        PaymentDetail.UserName = dr["UserName"].Equals(DBNull.Value) ? String.Empty : Convert.ToString(dr["UserName"]);
                //        // PaymentDetail.Tin = dr["Tin"].Equals(DBNull.Value) ? String.Empty : Convert.ToString(dr["Tin"]);
                //        PaymentDetail.AccountNumberCBE = dr["AccountNumberCBE"].Equals(DBNull.Value) ? String.Empty : Convert.ToString(dr["AccountNumberCBE"]);
                //        PaymentDetail.ServiceTaken = dr["ServiceTaken"].Equals(DBNull.Value) ? String.Empty : Convert.ToString(dr["ServiceTaken"]);
                //        PaymentDetail.IsPaid = dr["ISPaid"].Equals(DBNull.Value) ? false : Convert.ToBoolean(dr["ISPaid"].ToString());
                //        PaymentDetail.IsVoid = dr["IsVoid"].Equals(DBNull.Value) ? false : Convert.ToBoolean(dr["IsVoid"].ToString());
                //        //DateTime MaximumDateBeforeRecaclulation = dr["DateWrittenOnModeOfCollection"].Equals(DBNull.Value) ? DateTime.MaxValue : Convert.ToDateTime(dr["DateWrittenOnModeOfCollection"]);
                //        //if (DateTime.Today > MaximumDateBeforeRecaclulation)
                //        //{
                //        //    RecaclulatePayment(OrderId);
                //        //    GetOrderByInvoiceNo(OrderId);
                //        //}
                //        PaymentDetail.ErrorCode = 200;
                //        PaymentDetail.ErrorMessage = "";
                //    }
                //    else
                //    {
                //        PaymentDetail.ErrorCode = 101;
                //        PaymentDetail.ErrorMessage = "";
                //    }
                //    return PaymentDetail;
            }
            catch (Exception ex)
            {
                //Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                throw new Exception("Error in GetPaymentByReceiptNo: " + ex.Message);
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
        }
        public PaymentReturnValue GetOrderByInvoiceNo(int OrderId)
        {
            var connection = new SqlConnection(_ConnectionString);
            var strSQL = @"  SELECT top(1) Payment.Id AS OrderId, Payment.DatePrepared AS OrderDate, Payment.IsPaid, Payment.Void AS IsVoid, 
										 TotalAmount =Payment.TotalPayment, DATEDIFF(DAY,Payment.DatePrepared,GETDATE()) NoOfDays,
                                        'CustomerName'=  BusinessNameAmh , 
                                        'ServiceTaken'=  'Tax Payment',
                                        'Tin'= Tin ,
                                          Payment.AccountNumber, MaximumDateBeforeRecaclulation
                            FROM 
                                Payment WHERE  Payment.ID = @OrderId
                            Group by 
                                Payment.Id, ReceiptNo, Payment.DatePaid, CustomerId, Payment.TotalPayment,Payment.BusinessNameAmh,
							    IsPaid, Payment.Void, Payment.DatePrepared, checkno ,AccountNumber, MaximumDateBeforeRecaclulation ,Payment.Tin                      
                            order by DatePrepared desc  ";
            var command = new SqlCommand { CommandText = strSQL, CommandType = CommandType.Text };
            var dTable = new DataTable("Order");
            var adapter = new SqlDataAdapter(command);
            command.Connection = connection;

            var PaymentDetail = new PaymentReturnValue();
            try
            {
                command.Parameters.Add(new SqlParameter("@OrderId", OrderId));
                connection.Open();
                adapter.Fill(dTable);
                if (dTable.Rows.Count > 0)
                {
                    var dr = dTable.Rows[0];
                    PaymentDetail.OrderId = Convert.ToInt32(dr["OrderId"]);
                    PaymentDetail.OrderedDate = dr["OrderDate"].Equals(DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(dr["OrderDate"]);
                    PaymentDetail.TotalAmount = dr["TotalAmount"].Equals(DBNull.Value) ? 0 : Convert.ToDecimal(dr["TotalAmount"]);
                    PaymentDetail.CustomerName = dr["CustomerName"].Equals(DBNull.Value) ? String.Empty : Convert.ToString(dr["CustomerName"]);
                    PaymentDetail.Tin = dr["Tin"].Equals(DBNull.Value) ? String.Empty : Convert.ToString(dr["Tin"]);
                    PaymentDetail.AccountNumber = dr["AccountNumber"].Equals(DBNull.Value) ? String.Empty : Convert.ToString(dr["AccountNumber"]);
                    PaymentDetail.ServiceTaken = dr["ServiceTaken"].Equals(DBNull.Value) ? String.Empty : Convert.ToString(dr["ServiceTaken"]);
                    PaymentDetail.IsPaid = dr["ISPaid"].Equals(DBNull.Value) ? false : Convert.ToBoolean(dr["ISPaid"].ToString());
                    PaymentDetail.IsVoid = dr["IsVoid"].Equals(DBNull.Value) ? false : Convert.ToBoolean(dr["IsVoid"].ToString());
                    PaymentDetail.NoOfDays = dr["NoOfDays"].Equals(DBNull.Value) ? 0 : Convert.ToInt16(dr["NoOfDays"].ToString());

                    //DateTime MaximumDateBeforeRecaclulation = dr["MaximumDateBeforeRecaclulation"].Equals(DBNull.Value) ? DateTime.MaxValue : Convert.ToDateTime(dr["MaximumDateBeforeRecaclulation"]);
                    //if (DateTime.Today > MaximumDateBeforeRecaclulation)
                    //{
                    //    RecaclulatePayment(OrderId);
                    //    GetOrderByInvoiceNo(OrderId);
                    //}
                    PaymentDetail.ErrorCode = 200;
                    PaymentDetail.ErrorMessage = "";
                }
                else
                {
                    PaymentDetail.ErrorCode = 101;
                    PaymentDetail.ErrorMessage = "";
                }
                return PaymentDetail;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetPaymentByReceiptNo: " + ex.Message);
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
        }
        public Orders GetDetailByInvoiceNo(int OrderId)
        {
            var connection = new SqlConnection(_ConnectionString);
            var strSQL = @"SELECT Id
                            From Payment
                            WHERE  Payment.Id = @OrderId ";
            var command = new SqlCommand { CommandText = strSQL, CommandType = CommandType.Text };
            var dTable = new DataTable("Order");
            var adapter = new SqlDataAdapter(command);
            command.Connection = connection;

            var objOrders = new Orders();
            try
            {
                command.Parameters.Add(new SqlParameter("@OrderId", OrderId));
                connection.Open();
                adapter.Fill(dTable);
                if (dTable.Rows.Count > 0)
                {
                    var dr = dTable.Rows[0];
                    objOrders.InvoiceNo = Convert.ToInt32(dr["Id"]);
                }
                return objOrders;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetPaymentByReceiptNo: " + ex.Message);
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
        }
        public PaymentReturnValueEtSwitch GetOrderByInvoiceNoForEtSwitch(int OrderId, string ETSRequestID)
        {
            var connection = new SqlConnection(_ConnectionString);
            var strSQL = @"  SELECT top(1) Payment.Id AS OrderId, Payment.DatePrepared AS OrderDate, Payment.IsPaid, Payment.Void AS IsVoid, 
										 TotalAmount =Payment.TotalPayment, DATEDIFF(DAY,Payment.DatePrepared,GETDATE()) NoOfDays,
                                        'CustomerName'=  BusinessNameAmh , 
                                        'ServiceTaken'=  'Tax Payment',
                                        'Tin'= Tin ,
                                          Payment.AccountNumber, MaximumDateBeforeRecaclulation
                            FROM 
                                Payment WHERE  Payment.ID = @OrderId
                            Group by 
                                Payment.Id, ReceiptNo, Payment.DatePaid, CustomerId, Payment.TotalPayment,Payment.BusinessNameAmh,
							    IsPaid, Payment.Void, Payment.DatePrepared, checkno ,AccountNumber, MaximumDateBeforeRecaclulation ,Payment.Tin  
                            order by DatePrepared desc  ";
            var command = new SqlCommand { CommandText = strSQL, CommandType = CommandType.Text };
            var dTable = new DataTable("Order");
            var adapter = new SqlDataAdapter(command);
            command.Connection = connection;

            var PaymentDetail = new PaymentReturnValueEtSwitch();
            try
            {
                command.Parameters.Add(new SqlParameter("@OrderId", OrderId));
                connection.Open();
                adapter.Fill(dTable);
                if (dTable.Rows.Count > 0)
                {
                    var dr = dTable.Rows[0];
                    PaymentDetail.ETSRequestID = ETSRequestID;
                    PaymentDetail.OrderId = Convert.ToInt32(dr["OrderId"]);
                    PaymentDetail.OrderedDate = dr["OrderDate"].Equals(DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(dr["OrderDate"]);
                    PaymentDetail.TotalAmount = dr["TotalAmount"].Equals(DBNull.Value) ? 0 : Convert.ToDecimal(dr["TotalAmount"]);
                    PaymentDetail.CustomerName = dr["CustomerName"].Equals(DBNull.Value) ? String.Empty : Convert.ToString(dr["CustomerName"]);
                    PaymentDetail.Tin = dr["Tin"].Equals(DBNull.Value) ? String.Empty : Convert.ToString(dr["Tin"]);
                    PaymentDetail.AccountNumber = dr["AccountNumber"].Equals(DBNull.Value) ? String.Empty : Convert.ToString(dr["AccountNumber"]);
                    PaymentDetail.ServiceTaken = dr["ServiceTaken"].Equals(DBNull.Value) ? String.Empty : Convert.ToString(dr["ServiceTaken"]);
                    PaymentDetail.IsPaid = dr["ISPaid"].Equals(DBNull.Value) ? false : Convert.ToBoolean(dr["ISPaid"].ToString());
                    PaymentDetail.IsVoid = dr["IsVoid"].Equals(DBNull.Value) ? false : Convert.ToBoolean(dr["IsVoid"].ToString());
                    PaymentDetail.NoOfDays = dr["NoOfDays"].Equals(DBNull.Value) ? 0 : Convert.ToInt16(dr["NoOfDays"].ToString());

                    //DateTime MaximumDateBeforeRecaclulation = dr["MaximumDateBeforeRecaclulation"].Equals(DBNull.Value) ? DateTime.MaxValue : Convert.ToDateTime(dr["MaximumDateBeforeRecaclulation"]);
                    //if (DateTime.Today > MaximumDateBeforeRecaclulation)
                    //{
                    //    RecaclulatePayment(OrderId);
                    //    GetOrderByInvoiceNo(OrderId);
                    //}
                    PaymentDetail.ErrorCode = 200;
                    PaymentDetail.ErrorMessage = "";
                }
                else
                {
                    PaymentDetail.ErrorCode = 101;
                    PaymentDetail.ErrorMessage = "";
                }
                return PaymentDetail;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetPaymentByReceiptNo: " + ex.Message);
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
        }
        public bool CheckReceiptNoExist(string ReceiptNo)
        {
            var connection = new SqlConnection(ConnectionString);
            var strExists = @"Select Id  from Payment where ReceiptNo  = @ReceiptNo";
            var command = new SqlCommand { CommandText = strExists, CommandType = CommandType.Text };
            command.Connection = connection;
            try
            {
                connection.Open();
                command.Parameters.Add(new SqlParameter("@ReceiptNo", ReceiptNo));
                var obj = command.ExecuteScalar();
                return obj != null;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
        }
        public bool isValidOrderNo(int PaymentOrderNumber)
        {
            var connection = new SqlConnection(ConnectionString);
            var strExists = string.Empty;
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["AddisEnabled"]) || Convert.ToBoolean(ConfigurationManager.AppSettings["DireDawaEnabled"]) || Convert.ToBoolean(ConfigurationManager.AppSettings["SomaliEnabled"]))
            {
                strExists = @"SELECT Id FROM  Payment WHERE Id = @PaymentOrderNumber ";
            }
            else if (Convert.ToBoolean(ConfigurationManager.AppSettings["DarsEnabled"]) || Convert.ToBoolean(ConfigurationManager.AppSettings["MotriEnabled"]))
            {
                strExists = @"Select OrderID  from [Order] where OrderId = @PaymentOrderNumber ";
            }
            else
            {
                strExists = @" SELECT Id FROM  Payment WHERE Id = @PaymentOrderNumber ";
            }
            //var strExists = @" SELECT Id FROM  Payment WHERE Id = @PaymentOrderNumber ";
            var command = new SqlCommand { CommandText = strExists, CommandType = CommandType.Text };
            command.Connection = connection;
            try
            {
                connection.Open();
                command.Parameters.Add(new SqlParameter("@PaymentOrderNumber", PaymentOrderNumber));
                var obj = command.ExecuteScalar();
                return obj != null;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
        }
        public bool isViodOrderNo(int PaymentOrderNumber)
        {
            var connection = new SqlConnection(ConnectionString);
            var strExists = string.Empty;
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["AddisEnabled"]) || Convert.ToBoolean(ConfigurationManager.AppSettings["DireDawaEnabled"]) || Convert.ToBoolean(ConfigurationManager.AppSettings["SomaliEnabled"]))
            {
                //strExists = @"Select OrderID  from [Order] where OrderId = @PaymentOrderNumber ";
                strExists = @"SELECT Id FROM  Payment WHERE Id = @PaymentOrderNumber AND Isnull(Void,0)=1 ";
            }
            else if (Convert.ToBoolean(ConfigurationManager.AppSettings["DarsEnabled"]) || Convert.ToBoolean(ConfigurationManager.AppSettings["MotriEnabled"]))
            {
                strExists = @"Select OrderID  from [Order] where OrderId  = @PaymentOrderNumber AND isnull(IsVoid,0)=1  ";
            }
            else
            {
                strExists = @" SELECT Id FROM  Payment WHERE Id = @PaymentOrderNumber AND Isnull(Void,0)=1 ";
            }
            //var strExists = @" SELECT Id FROM  Payment WHERE Id = @PaymentOrderNumber AND Isnull(Void,0)=1 ";
            var command = new SqlCommand { CommandText = strExists, CommandType = CommandType.Text };
            command.Connection = connection;
            try
            {
                connection.Open();
                command.Parameters.Add(new SqlParameter("@PaymentOrderNumber", PaymentOrderNumber));
                var obj = command.ExecuteScalar();
                return obj != null;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
        }
        public Tuple<string, string, decimal> GetTaxPayerInfo(int PaymentOrderNumber)
        {
            string Tin = "";
            string TellPhoneNo = "";
            decimal TotalPayment = 0;

            var connection = new SqlConnection(_ConnectionString);
            var strSQL = @"  SELECT Tin, TeleNo,TotalPayment FROM  Payment WHERE Id = @PaymentOrderNumber ";
            var command = new SqlCommand { CommandText = strSQL, CommandType = CommandType.Text };
            var dTable = new DataTable("Order");
            var adapter = new SqlDataAdapter(command);
            command.Connection = connection;
            try
            {
                command.Parameters.Add(new SqlParameter("@PaymentOrderNumber", PaymentOrderNumber));
                connection.Open();
                adapter.Fill(dTable);
                if (dTable.Rows.Count > 0)
                {
                    Tin = dTable.Rows[0]["Tin"].Equals(DBNull.Value) ? String.Empty : Convert.ToString(dTable.Rows[0]["Tin"]);
                    TellPhoneNo = dTable.Rows[0]["TeleNo"].Equals(DBNull.Value) ? String.Empty : Convert.ToString(dTable.Rows[0]["TeleNo"]);
                    TotalPayment = dTable.Rows[0]["TotalPayment"].Equals(DBNull.Value) ? 0 : Convert.ToDecimal(dTable.Rows[0]["TotalPayment"]);
                    return Tuple.Create(Tin, TellPhoneNo, TotalPayment);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
            return Tuple.Create(Tin, TellPhoneNo, TotalPayment);
        }
        public bool CheckPaymentIsDoneBythisPaymentOrderNo(int PaymentOrderNumber)
        {
            var connection = new SqlConnection(ConnectionString);
            var strExists = string.Empty;
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["AddisEnabled"]) || Convert.ToBoolean(ConfigurationManager.AppSettings["DireDawaEnabled"]) || Convert.ToBoolean(ConfigurationManager.AppSettings["SomaliEnabled"]))
            {
                strExists = @"Select Id  from Payment where IsPaid = 1 AND Id  = @PaymentOrderNumber ";
            }
            else if (Convert.ToBoolean(ConfigurationManager.AppSettings["DarsEnabled"]) || Convert.ToBoolean(ConfigurationManager.AppSettings["MotriEnabled"]))
            {
                strExists = @"Select OrderID  from Order where IsPaid = 1 AND OrderId  = @PaymentOrderNumber ";
            }
            else
            {
                strExists = @"Select Id  from Payment where IsPaid = 1 AND Id  = @PaymentOrderNumber ";
            }
            var command = new SqlCommand { CommandText = strExists, CommandType = CommandType.Text };
            command.Connection = connection;
            try
            {
                connection.Open();
                command.Parameters.Add(new SqlParameter("@PaymentOrderNumber", PaymentOrderNumber));
                var obj = command.ExecuteScalar();
                return obj != null;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
        }
        public List<PaymentList> GetPayments(DateTime PaymentDateFrom, DateTime PaymentDateTo, bool IsVoid, string BankCode)
        {
            var RecordsList = new List<PaymentList>();
            var connection = new SqlConnection(_ConnectionString);
            SqlDataReader dr = null;
            var command = new SqlCommand();
            var strSQL = @" SELECT Payment.CashierName,Payment.ID as PaymentOrderNo, ReceiptNo,DatePaid, 										
                                        TotalAmount =Payment.TotalPayment,  
                                        'CustomerName'=  BusinessNameAmh , 'Tin'= Tin ,
									     CheckNo as ChequeNo
                            FROM  Payment INNER JOIN PaymentDetail ON   Payment.ID=PaymentDetail.PaymentId
                                  AND Ispaid=1 AND 1 = 1  AND  ( _X )  AND
                                 (cast(DatePaid as date) >=  @PaymentDateFrom) AND (cast(DatePaid as date) <= @PaymentDateTo) AND (ReceiptNo <> '0')  AND BankCode= @BankCode	    
                            Group by Payment.ID,ReceiptNo,DatePaid, Payment.CashierName, checkno , CustomerId, Payment.TotalPayment,Payment.BusinessNameAmh,Payment.Tin
                            order by DatePaid desc  ";

            strSQL = strSQL.Replace("_X", !IsVoid ? "Void=0 or Void is null" : "Void=1");
            command.CommandText = strSQL;
            command.CommandType = CommandType.Text;
            command.Connection = connection;
            try
            {
                command.Parameters.Add(new SqlParameter("@PaymentDateFrom", PaymentDateFrom.ToString("yyy-MM-dd")));
                command.Parameters.Add(new SqlParameter("@PaymentDateTo", PaymentDateTo.ToString("yyy-MM-dd")));
                command.Parameters.Add(new SqlParameter("@BankCode", BankCode));
                connection.Open();
                dr = command.ExecuteReader();
                while (dr.Read())
                {
                    var pmt = new PaymentList();
                    pmt.CashierName = dr["CashierName"].Equals(DBNull.Value) ? string.Empty : dr["CashierName"].ToString();
                    pmt.CustomerName = dr["CustomerName"].Equals(DBNull.Value) ? string.Empty : dr["CustomerName"].ToString();
                    pmt.ReceiptNo = dr["ReceiptNo"].Equals(DBNull.Value) ? string.Empty : dr["ReceiptNo"].ToString();
                    pmt.PaymentDate = dr["DatePaid"].Equals(DBNull.Value) ? DateTime.MinValue : (DateTime)dr["DatePaid"];
                    pmt.TotalPayment = dr["TotalPayment"].Equals(DBNull.Value) ? 0 : Convert.ToDecimal(dr["TotalPayment"]);
                    pmt.Tin = dr["Tin"].Equals(DBNull.Value) ? string.Empty : dr["Tin"].ToString();
                    pmt.PaymentOrderNo = dr["PaymentOrderNo"].Equals(DBNull.Value) ? 0 : (int)dr["PaymentOrderNo"];
                    pmt.ChequeNo = dr["ChequeNo"].Equals(DBNull.Value) ? string.Empty : dr["ChequeNo"].ToString();
                    RecordsList.Add(pmt);
                }
                return RecordsList;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetPayments: " + ex.Message);
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
        }

        public Orders GetServiceApplicationDetailByInvoiceNo(int OrderId)
        {
            var connection = new SqlConnection(_ConnectionString);
            //var strSQL = @"SELECT  AccountNumber, ServiceApplicationGuid,OrderId,MobileNo
            //                From [Order]
            //                WHERE  [Order].OrderID = @OrderId ";
            var strSQl = @"SELECT  AccountNumber, ServiceApplicationGuid,OrderId,MobileNo,Sum(isnull(OrderDetail.Amount,0.00)) as TotalAmount,
                                   (SELECT top(1) ServiceName FROM  [Service] where ServiceTypeCode= [Order].ServiceTypeCode) as ServiceTakenAm,
                                   (SELECT top(1) ServiceNameEnglish FROM  [Service] where ServiceTypeCode= [Order].ServiceTypeCode) as ServiceTakenEng,ReceiptNo
                                                  From [Order]
							inner join OrderDetail on [Order].OrderGuid=OrderDetail.OrderGuid
                            WHERE  [Order].OrderID = @OrderId
							Group by [Order].OrderID,[Order].ServiceApplicationGuid,[Order].AccountNumber, [Order].MobileNo,[Order].ServiceTypeCode,[Order].ReceiptNo";
            var command = new SqlCommand { CommandText = strSQl, CommandType = CommandType.Text };
            var dTable = new DataTable("Order");
            var adapter = new SqlDataAdapter(command);
            command.Connection = connection;

            var objOrders = new Orders();
            try
            {
                command.Parameters.Add(new SqlParameter("@OrderId", OrderId));
                connection.Open();
                adapter.Fill(dTable);
                if (dTable.Rows.Count > 0)
                {
                    var dr = dTable.Rows[0];
                    objOrders.Parent_guid = dr["ServiceApplicationGuid"].ToString();
                    objOrders.AccountNo = dr["AccountNumber"].ToString();
                    objOrders.ServiceGuid = new Guid(dr["ServiceApplicationGuid"].ToString());
                    objOrders.TellNo = dr["MobileNo"].Equals(DBNull.Value) ? string.Empty : dr["MobileNo"].ToString();
                    objOrders.Amount = dr["TotalAmount"].Equals(DBNull.Value) ? 0 : Convert.ToDecimal(dr["TotalAmount"]);
                    objOrders.ReceiptNo = dr["ReceiptNo"].Equals(DBNull.Value) ? string.Empty : dr["ReceiptNo"].ToString();
                    objOrders.ServiceName = dr["ServiceTakenAm"].Equals(DBNull.Value) ? string.Empty : dr["ServiceTakenAm"].ToString();
                    objOrders.ServiceNameEnglish = dr["ServiceTakenEng"].Equals(DBNull.Value) ? string.Empty : dr["ServiceTakenEng"].ToString();
                }
                return objOrders;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                throw new Exception("Error in GetPaymentByReceiptNo: " + ex.Message);
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
        }

    }
}