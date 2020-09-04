using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fastdo.backendsys;
using Fastdo.backendsys.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Diagnostics;

namespace Microsoft.AspNetCore.Builder
{
    public static class UseServices
    {
        public static IApplicationBuilder _UseServicesStarter(this IApplicationBuilder app,IServiceProvider serviceProvider)
        {
            RequestStaticServices.Init(serviceProvider);
            return app;
        }
        private static async Task<IApplicationBuilder> _UseInitalSeeds_In_Developement(this IApplicationBuilder app)
        {
            //add areas and adminerUser
            await DataSeeder.SeedBasicData();
            //add default data to system
            await DataSeeder.SeedDefaultData();
            return app;
        }
        private static async Task<IApplicationBuilder> _UseInitalSeeds_In_Production(this IApplicationBuilder app)
        {
            //add areas and adminerUser
            await DataSeeder.SeedBasicData();
            return app;
        }
        public static IApplicationBuilder _UseMyDbConfigStarter(this IApplicationBuilder app, IHostingEnvironment env)
        {
            //RequestStaticServices.GetDbContext().Database.Migrate();
            //DbServicesFuncs.ResetData().Wait();
            var _roleManager = RequestStaticServices.GetRoleManager();
            _roleManager._addRoles(new List<string> { Variables.adminer, Variables.pharmacier, Variables.stocker }).Wait();
            if (env.IsDevelopment())
            {               
                app._UseInitalSeeds_In_Developement().Wait();
            }
            else if(env.IsProduction())
            {
                app._UseInitalSeeds_In_Production().Wait();
            }
            return app;
        }
        public static IApplicationBuilder _UseExceptions(this IApplicationBuilder app, IHostingEnvironment env)
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
                        if (context.Request.Path.Value.Contains("/AdminPanel", StringComparison.OrdinalIgnoreCase) &&
                            !context.Request.Path.Value.Contains("/api/", StringComparison.OrdinalIgnoreCase))
                        {
                            context.Response.Redirect("/AdminPanel/Home/Error");
                        }
                        else
                        {
                            context.Response.StatusCode = 500;
                            await context.Response.WriteJsonAsync(Functions.MakeError("unhandled exception happend,try again"));
                        }                     
                        
                    });
                  
                });
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.

            }
            return app;
        }
    }
}