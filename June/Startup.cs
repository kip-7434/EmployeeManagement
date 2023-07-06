using June.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
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

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 3;
                options.Password.RequiredUniqueChars = 1;
                options.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            

            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddXmlSerializerFormatters();

            services.AddAuthentication().AddGoogle(options =>
            {
                    options.ClientId = " 888856716115 - g8c1a1j23bi6vv3cse6bhvao65ok4bhn.apps.googleusercontent.com";

                    options.ClientSecret = "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";
                });

            //lifspan of confirmation link
            services.Configure < DataProtectionTokenProviderOptions>(o =>
                o.TokenLifespan = TimeSpan.FromHours(5));

            //change access denied route
            //services.ConfigureApplicationCookie(options =>
            //{
            //    options.AccessDeniedPath = new PathString("/Administration/AccessDenied");
            //});

            //creating and registering claims policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("DeleteRolePolicy",
                    policy => policy.RequireClaim("Delete Role")
                   .RequireClaim("Create Role") );
                options.AddPolicy("EditRolePolicy",
                    policy => policy.RequireClaim("Edit Role"));
            });

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
            app.UseAuthentication();
            app.UseMvc( routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");

            });
        }
    }

    internal class AuthorizationFilter : IFilterMetadata
    {
    }
}
  