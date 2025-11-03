using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JobFinder.Startup))]
namespace JobFinder
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
