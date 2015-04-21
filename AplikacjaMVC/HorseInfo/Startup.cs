using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HorseInfo.Startup))]
namespace HorseInfo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
