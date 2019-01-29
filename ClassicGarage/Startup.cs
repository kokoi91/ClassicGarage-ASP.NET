using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ClassicGarage.Startup))]
namespace ClassicGarage
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
