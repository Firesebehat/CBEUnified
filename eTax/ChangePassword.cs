using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Security;
using MotiSectorAPI.Model;

namespace MotiSectorAPI.Common
{
    public class ChangePassword
    {
        public ActionResult DoChangePassword(string strUserName, string strOldPassword, string strNewPassword,
            bool bIsNewOrReset)
        {
            const string strDefaultPassword = "Ch@ngeP@55w0rd";
            var result = new ActionResult();
            var passwordSetting = CPasswordManager.GetPasswordSetting();
            var sbPasswordRegx = new StringBuilder(string.Empty);
            try
            {
                if (bIsNewOrReset)
                {
                    var usr = Membership.GetUser(strUserName);
                    var strPwd = usr.GetPassword();
                    if (strPwd != strDefaultPassword)
                    {
                        result.ErrorMessage = "Account is not new or reset";
                        result.ErrorCode = 1;
                        result.IsSuccess = false;
                        return result;
                    }
                }
                if (!bIsNewOrReset)
                    if (strOldPassword.Trim().Length == 0)
                    {
                        result.ErrorMessage = "Old passowrd can not be empty!";
                        result.ErrorCode = 2;
                        result.IsSuccess = false;
                        return result;
                    }

                var strPassword = bIsNewOrReset ? strDefaultPassword : strOldPassword.Trim();
                if (!Membership.ValidateUser(strUserName, strPassword))
                {
                    result.ErrorMessage = "Old/Default password is not correct!";
                    result.ErrorCode = 3;
                    result.IsSuccess = false;
                    return result;
                }

                if (strNewPassword.Trim().Length == 0)
                {
                    result.ErrorMessage = "New passoword can not be empty!";
                    result.ErrorCode = 4;
                    result.IsSuccess = false;
                    return result;
                }


                if (!bIsNewOrReset)
                    if (strNewPassword.Trim() == strOldPassword.Trim())
                    {
                        result.ErrorMessage = "The Old and new Password can not be the same!";
                        result.ErrorCode = 5;
                        result.IsSuccess = false;
                        return result;
                    }

                if (strNewPassword.Trim().Length < passwordSetting.MinLength)
                {
                    result.ErrorMessage = string.Format("Password must be at least {0} characters long",
                        passwordSetting.MinLength);
                    result.IsSuccess = false;
                    result.ErrorCode = 6;
                    return result;
                }
                if (strNewPassword.Trim().Length > passwordSetting.MaxLength)
                {
                    result.ErrorMessage = string.Format("Password can not be more than {0} characters long",
                        passwordSetting.MaxLength);
                    result.IsSuccess = false;
                    result.ErrorCode = 7;
                    return result;
                }


                //a-z characters
                sbPasswordRegx.Append(@"(?=.*[a-z])");

                //A-Z length
                sbPasswordRegx.Append(@"(?=(?:.*?[A-Z]){" + passwordSetting.UpperLength + "})");

                ////special characters length
                //sbPasswordRegx.Append(@"(?=(?:.*?[" + passwordSetting.SpecialChars + "]){" + passwordSetting.SpecialLength + "})");

                //sbPasswordRegx.Append(@"(?!.*\s)[0-9a-zA-Z" + passwordSetting.SpecialChars + "]*$");

                if (!Regex.IsMatch(strNewPassword.Trim(), sbPasswordRegx.ToString()))
                {
                    result.ErrorMessage = "Password does not conform to password policy!";
                    result.IsSuccess = false;
                    result.ErrorCode = 8;
                    return result;
                }

                var usrInfo = Membership.GetUser(strUserName);
                usrInfo.IsApproved = true;

                if (usrInfo.ChangePassword(strPassword, strNewPassword))
                {
                    Membership.UpdateUser(usrInfo);
                    //CMsgBar.ShowMessage("Password was Changed Successfully", pnlMsg, lblError);
                    result.ErrorMessage = "Success";
                    result.ErrorCode = 200;
                    result.IsSuccess = true;
                    return result;
                }

                result.ErrorMessage = "Unhandled Error";
                result.ErrorCode = 500;
                result.IsSuccess = false;
                return result;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
                result.ErrorCode = 500;
                result.IsSuccess = false;
                return result;
            }
        }
    }
}