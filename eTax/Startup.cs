using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EtTaxOauth2.Startup))]
namespace EtTaxOauth2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
