using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ELearningProject.Startup))]
namespace ELearningProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
