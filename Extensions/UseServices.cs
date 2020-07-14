using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System_Back_End;
using System_Back_End.Services;

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
            DbServicesFuncs.ResetData().Wait();
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
    }
}