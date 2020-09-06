
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using WebApplication4.Models;

namespace WebApplication4
{
    public class Startup
    {

        public IConfiguration Configuration { get; }
        
        public Startup(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<Account, AppRole>(option =>
            {
                option.Password.RequireNonAlphanumeric = false;
                option.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<DatabaseContext>();


            services.AddSession();

            services.AddRazorPages().AddRazorRuntimeCompilation();

            services.AddControllersWithViews();

            services.AddMvc(option => option.EnableEndpointRouting = false);

            var connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connection));

            services.AddSingleton<HtmlEncoder>(
            HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin,
                                            UnicodeRanges.Arabic }));

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
         {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            

            app.UseSession();

            app.UseMvc();

            app.UseStaticFiles();
          
           
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });

            app.UseAuthentication();


        }
    }
}
