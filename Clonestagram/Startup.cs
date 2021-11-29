using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Clonestagram.Startup))]
namespace Clonestagram
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
