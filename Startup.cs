using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Models;
using System_Back_End.Providers;
using Microsoft.AspNetCore.Mvc;

namespace System_Back_End
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
                    options.UseSqlServer(Configuration.GetConnectionString("AWS_fastdo_db"),
                    builder=> {
                    builder.MigrationsAssembly("System_Back_End");
                    });
                    /*options.UseSqlite(Configuration.GetConnectionString("sysSqlite"), builder => {
                        builder.MigrationsAssembly("System_Back_End");
                    });*/
                }
                else
                {
                     options.UseSqlite(Configuration.GetConnectionString("sysSqlite"), builder => {
                        builder.MigrationsAssembly("System_Back_End");
                    });
                    /*options.UseSqlServer(Configuration.GetConnectionString("AWS_fastdo_db"), builder => {
                        builder.MigrationsAssembly("System_Back_End");
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteJsonAsync(Functions.MakeError("unhandled exception happend,try again"));
                    });
                });
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app._UseServicesStarter(serviceProvider);
            app._UseMyDbConfigStarter(env);
            app.UseCors(Variables.corePolicy);
            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                
            });
        }
    }
}
