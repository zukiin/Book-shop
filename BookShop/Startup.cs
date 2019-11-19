using BookShop.Models;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BookShop.Startup))]
namespace BookShop
{
    public partial class Startup
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRole();
            CreateUser();
        }
    }
}
