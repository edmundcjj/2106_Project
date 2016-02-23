using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OneStopTourist.Startup))]
namespace OneStopTourist
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
