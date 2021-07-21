using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using POAM.Code;
using POAM.Models;

namespace POAM
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
           {
               options.IdleTimeout = TimeSpan.FromMinutes(20);
               options.Cookie.HttpOnly = true;
           }
                );
                

           // services.AddMvc();
            services.AddMvc(options => options.EnableEndpointRouting = false);

      


            #region Authentication_Autorization
            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            services.AddHttpContextAccessor();
            // configure basic authentication 
            services.AddAuthentication("BasicAuthentication")
               .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
            // configure DI for application services
            services.AddScoped<IUserService, UserService>();
            #endregion


            //var connection = @"Server== ftasqldev,4173; Database = POAM; Trusted_Connection = True;ConnectRetryCount=0";
            services.AddDbContext<POAMContext>(options => options.UseSqlServer(Configuration.GetConnectionString("POAMDatabase")));

        }
       
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
              
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseSession();

            #region AuthenticationAughorization
            app.UseStatusCodePages(async context => {
                if (context.HttpContext.Response.StatusCode == 401)
                {
                    // your redirect
                    context.HttpContext.Response.Redirect("/Home/AccessDenied");
                }
                if (context.HttpContext.Response.StatusCode == 404)
                {
                    // your redirect
                   context.HttpContext.Response.Redirect("/Home/AccessDenied");
                }
            });
            app.UseAuthentication();
            #endregion

            SessionValues.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
