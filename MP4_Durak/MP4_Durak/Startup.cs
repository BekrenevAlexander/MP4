using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MP4_Durak.Startup))]
namespace MP4_Durak
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
