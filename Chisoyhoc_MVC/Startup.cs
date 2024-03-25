using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Chisoyhoc_MVC.Startup))]
namespace Chisoyhoc_MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
