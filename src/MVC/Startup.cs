using Microsoft.EntityFrameworkCore;
using MVC.Models;

namespace MVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<PitchforkContext>(options =>
                options.UseLazyLoadingProxies().UseSqlServer(Configuration.GetConnectionString("DefaultConnectionString")));

            services.AddControllersWithViews();
            services.AddAuthorization();
        }
    }
}