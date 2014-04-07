using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AzureVideoStreaming.Web.Startup))]
namespace AzureVideoStreaming.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
