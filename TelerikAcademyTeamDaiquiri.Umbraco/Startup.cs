using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TelerikAcademyTeamDaiquiri.Umbraco.Startup))]
namespace TelerikAcademyTeamDaiquiri.Umbraco
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
