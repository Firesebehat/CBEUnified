using System.Security.Principal;

namespace eTaxAPI.Filters
{
    /// <summary>
    ///     Basic Authentication identity
    /// </summary>
    public class BasicAuthenticationIdentity : GenericIdentity
    {
        public BasicAuthenticationIdentity(string userName, string password)
            : base(userName, "Basic")
        {
            Password = password;
            UserName = userName;
        }

        public string Password { get; set; }

        public string UserName { get; set; }
        public int UserId { get; set; } //not needed
    }
}