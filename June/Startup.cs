using June.models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace June
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration config)//dependecy injection 
        {
            _config = config;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(_config.GetConnectionString("EmployeeDBConnection"), sqlServerOptions =>
                     {
                         sqlServerOptions.EnableRetryOnFailure();
                         }));
            services.AddMvc(option => option.EnableEndpointRouting=false);
            services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();//1 midlleware plugin as ealy
            }

            //app.UseFileServer();
           
            app.UseStaticFiles();
            //app.UseMvcWithDefaultRoute();
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");

            });
        }
    }
}
  