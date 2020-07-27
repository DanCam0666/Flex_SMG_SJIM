using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Flex_SGM.Startup))]
namespace Flex_SGM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
