using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BiberDAMM.Startup))]
namespace BiberDAMM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
