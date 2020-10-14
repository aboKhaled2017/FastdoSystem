using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fastdo.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;

namespace Fastdo.backendsys.Services
{
    public static class DbServicesFuncs
    {
        private static readonly SysDbContext context = RequestStaticServices.GetDbContext();
        private static readonly UserManager<AppUser> _userManager = RequestStaticServices.GetUserManager();
        private static readonly RoleManager<IdentityRole> _roleManager = RequestStaticServices.GetRoleManager();
        private static readonly IHostingEnvironment env= RequestStaticServices.GetHostingEnv();
        public static async Task ResetData()
        {
            if (env.IsProduction()) return;
            var roles =_roleManager.Roles.ToList();
            roles.ForEach( role =>
            {
                _roleManager.DeleteAsync(role).Wait();
            });
            
            var users = _userManager.Users;
            users.ToList().ForEach(user =>
            {
                _userManager.DeleteAsync(user).Wait();
            });
            await context.SaveChangesAsync();        
        }
    }
}
