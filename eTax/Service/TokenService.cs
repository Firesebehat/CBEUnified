using System;
using System.Configuration;
using eTaxAPI.DataAccess;
using eTaxAPI.Models;

namespace eTaxAPI.Service
{
    public class TokenService
    {
        public static Token GenerateToken(string userName)
        {
            var token = Guid.NewGuid().ToString();
            var issuedOn = DateTime.Now;
            var expiredOn =
                DateTime.Now.AddSeconds(Convert.ToDouble(ConfigurationManager.AppSettings["AuthTokenExpiry"]));
            var tokendomain = new Token
            {
                UserName = userName, //redundent
                AuthToken = token,
                IssuedOn = issuedOn,
                ExpiresOn = expiredOn
            };
            var objToken = new TokenDA();

            objToken.Insert(tokendomain);

            var tokenModel = new Token
            {
                UserName = userName,
                IssuedOn = issuedOn,
                ExpiresOn = expiredOn,
                AuthToken = token
            };
            return tokenModel;
        }

        public static string ValidateToken(string tokenId)
        {
            var objToken = new TokenDA();
            var token = objToken.GetRecord(tokenId); //  

            if (token == null || DateTime.Now > token.ExpiresOn) return string.Empty;
            token.ExpiresOn =
                token.ExpiresOn.AddSeconds(Convert.ToDouble(ConfigurationManager.AppSettings["AuthTokenExpiry"]));
            objToken.Update(token.AuthToken, token.ExpiresOn);
            return token.UserName;
        }

        public bool Delete(string tokenId)
        {
            var objToken = new TokenDA();
            return objToken.Delete(tokenId);
        }
    }
}