using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Fastdo.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Fastdo.backendsys.Utilities;
using System.Net;
using NLog;
using System.IO;
using NHibernate.Util;

namespace Fastdo.backendsys
{
    public class Startup
    {
        public Startup(IConfiguration configuration,IHostingEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SysDbContext>(options => 
            {
                if(Env.IsDevelopment())
                {
                    /*options.UseSqlServer(Configuration.GetConnectionString("FastdoSQlServer"),
                    builder=> {
                    builder.MigrationsAssembly("Fastdo.backendsys");
                    });*/
                    options.UseSqlServer(Configuration.GetConnectionString("smarterFastdo"), builder => {
                        builder.MigrationsAssembly("Fastdo.backendsys");
                    });
                }
                else
                {
                    options.UseSqlServer(Configuration.GetConnectionString("FastdoSQlServer"), builder => {
                        builder.MigrationsAssembly("Fastdo.backendsys");
                    });
                    /*options.UseSqlServer(Configuration.GetConnectionString("smarterFastdo"), builder => {
                        builder.MigrationsAssembly("Fastdo.backendsys");
                    });*/
                }                       
            });           
            services._AddRepositories();
            services
                .AddIdentity<AppUser, IdentityRole>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<SysDbContext>();

            services._AddAutoMapper();
            services._AddSystemServices();
            services._AddSystemAuthentication();
            services._AddSystemAuthorizations();
            services.AddMvc(options=> {
                options.Conventions.Add(new ControllerDocumentationsConvensions());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddCors(options => {
                options.AddPolicy(Variables.corePolicy, builder => {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowCredentials();
                    builder.WithExposedHeaders(Variables.X_PaginationHeader);
                });
            });
            services.Configure<IdentityOptions>(op => {
                op.Password.RequireNonAlphanumeric = false;
                op.Password.RequireUppercase = false;
                op.Password.RequireLowercase = false;
                op.Password.RequireDigit = false;
                
            });
            services.Configure<DataProtectionTokenProviderOptions>(op =>
            {
                op.TokenLifespan = TimeSpan.FromDays(1);
            });
            services._AddSwaggr();
            //services._AddGraphQlServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,IServiceProvider serviceProvider)
        {
            app.UseCors(Variables.corePolicy);
            app._UserSwagger();
            app._UseExceptions(env);
            app.UseHsts();
            app.UseHttpsRedirection();
            app._UseServicesStarter(serviceProvider);
            app._UseMyDbConfigStarter(env);
            //app._UseQraphQl();
            //app._useCustomFunctionToBeImplemented(env);/*disable it*/
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseStatusCodePages(context => {
                if (context.HttpContext.Request.Path.Value.Contains("/AdminPanel",StringComparison.OrdinalIgnoreCase)&&
                    !context.HttpContext.Request.Path.Value.Contains("/api/", StringComparison.OrdinalIgnoreCase))
                {
                    var response = context.HttpContext.Response;
                    if (response.StatusCode == (int)HttpStatusCode.Unauthorized ||
                        response.StatusCode == (int)HttpStatusCode.Forbidden)
                        response.Redirect("/AdminPanel/Auth/Signin");
                }
                return Task.CompletedTask;
            });            
            app.UseMvc(routes =>
            {              
                routes.MapAreaRoute("AdminAreaRoute", "AdminPanel", "AdminPanel/{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute("default", "AdminPanel/{controller=Home}/{action=Index}/{id?}", new { Area="AdminPanel"});
            });
        }
    }
}
