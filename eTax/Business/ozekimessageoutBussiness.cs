using System;
using System.Collections.Generic;
using System.Configuration; 
using CUSTOR.DataAccess;
using System.Data.SqlClient;
using eTax.Model;
namespace CUSTOR.Bussiness
{
    public class ozekimessageoutBussiness
    {


        public bool Delete(string ID)
        {
            ozekimessageoutDataAccess objozekimessageoutDataAccess = new ozekimessageoutDataAccess();
            try
            {
                objozekimessageoutDataAccess.Delete(ID);
                return true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public ozekimessageout Getozekimessageout(string ID)
        {
            ozekimessageoutDataAccess objozekimessageoutDataAccess = new ozekimessageoutDataAccess();

            try
            {
                return objozekimessageoutDataAccess.GetRecord(ID);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public List<ozekimessageout> Getozekimessageouts()
        {
            ozekimessageoutDataAccess objozekimessageoutDataAccess = new ozekimessageoutDataAccess();

            try
            {
                return objozekimessageoutDataAccess.GetList();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public bool Exists(string ID)
        {
            ozekimessageoutDataAccess objozekimessageoutDataAccess = new ozekimessageoutDataAccess();
            try
            {
                return objozekimessageoutDataAccess.Exists(ID);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public bool Updateozekimessageout(ozekimessageout objozekimessageout)
        {
            ozekimessageoutDataAccess objozekimessageoutDataAccess = new ozekimessageoutDataAccess();
            try
            {
                objozekimessageoutDataAccess.Update(objozekimessageout);
                return true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        //public bool Insertozekimessageout(ozekimessageout objozekimessageout)
        //{
        //    ozekimessageoutDataAccess objozekimessageoutDataAccess = new ozekimessageoutDataAccess();
        //    try
        //    {
        //        objozekimessageoutDataAccess.Insert(objozekimessageout);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }
        //}
        public bool Insertozekimessageout(ozekimessageout objozekimessageout, SqlConnection connection)
        {
            ozekimessageoutDataAccess objozekimessageoutDataAccess = new ozekimessageoutDataAccess();
            try
            {
                objozekimessageoutDataAccess.Insert(objozekimessageout, connection);
                return true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


    }
}