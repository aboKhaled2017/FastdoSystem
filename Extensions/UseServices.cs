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
using GraphQL.Server;
using Fastdo.backendsys.Graphql;
using GraphQL.Server.Ui.Playground;

namespace Microsoft.AspNetCore.Builder
{
    public static class UseServices
    {
        public static IApplicationBuilder _UseServicesStarter(this IApplicationBuilder app,IServiceProvider serviceProvider)
        {
            RequestStaticServices.Init(serviceProvider);
            return app;
        }

        public static IApplicationBuilder _UseQraphQl(this IApplicationBuilder app)
        {
            app.UseGraphQL<FastdoSchema>();
            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions { Path="/graphql/ui"});
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
               // app._UseInitalSeeds_In_Developement().Wait();
            }
            else if(env.IsProduction())
            {
               // app._UseInitalSeeds_In_Production().Wait();
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
                        var exceptionDetails = context.Features.Get<IExceptionHandlerPathFeature>();
                        if (context.Request.Path.Value.Contains("/AdminPanel", StringComparison.OrdinalIgnoreCase) &&
                            !context.Request.Path.Value.Contains("/api/", StringComparison.OrdinalIgnoreCase))
                        {
                            context.Response.Redirect("/AdminPanel/Home/Error");
                        }
                        else
                        {
                            context.Response.StatusCode = 500;
                            await context.Response.WriteJsonAsync(Functions.MakeError(exceptionDetails.Error.Message));
                        }                     
                        
                    });
                  
                });
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            }
            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404 &&
                    !context.Request.Path.Value.Contains("/api/", StringComparison.OrdinalIgnoreCase) &&
                    context.Request.Path.Value.Contains("/AdminPanel/", StringComparison.OrdinalIgnoreCase))
                {
                    context.Request.Path = "/AdminPanel/Home/Error";
                    await next();
                }
            });
            return app;
        }
        public static IApplicationBuilder _UserSwagger(this IApplicationBuilder app)
        {
            app.UseOpenApi(c => { });
            app.UseSwaggerUi3();

            return app;
        }
        public static IApplicationBuilder _useCustomFunctionToBeImplemented(this IApplicationBuilder app,IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                var userManager = RequestStaticServices.GetUserManager();
                var context = RequestStaticServices.GetDbContext();
                var mainUser = userManager.FindByNameAsync("MahmoudAnwar").Result;
                /*if (user != null)
                {
                    userManager.DeleteAsync(user).Wait();
                    DataSeeder.SeedDefaultAdminstrator().Wait();
                }*/
                /*var user = new AppUser
                {
                    UserName = "ahmed251",
                    PhoneNumber = "01154585695"
                };
                userManager.CreateAsync(user, "AAaa123").Wait();
                userManager.AddToRoleAsync(user, Variables.adminer).Wait();
                userManager.AddClaimsAsync(user, new List<Claim> { 
                new Claim(Variables.AdminClaimsTypes.AdminType,AdminType.Administrator),
                new Claim(Variables.AdminClaimsTypes.Priviligs,
                $"{AdminerPreviligs.HaveControlOnAdminersPage.ToString()},{AdminerPreviligs.HaveControlOnStocksPage}")
                }).Wait();
                var admin = new Admin
                {
                    Name = "ahmed ali",
                    SuperAdminId = mainUser.Id,
                    User = user
                };
                context.Admins.Add(admin);
                context.SaveChanges();*/
            }
            return app;
        }
    }
}