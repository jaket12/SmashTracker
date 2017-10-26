using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SmashTracker.Startup))]
namespace SmashTracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
