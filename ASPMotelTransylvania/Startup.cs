using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ASPScenicHotel.Startup))]
namespace ASPScenicHotel
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
