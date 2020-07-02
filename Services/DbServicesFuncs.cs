using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Models;
using Microsoft.AspNetCore.Identity;

namespace System_Back_End.Services
{
    public static class DbServicesFuncs
    {
        private static readonly SysDbContext context = RequestStaticServices.GetDbContext();
        private static readonly UserManager<AppUser> _userManager = RequestStaticServices.GetUserManager();
        private static readonly RoleManager<IdentityRole> _roleManager = RequestStaticServices.GetRoleManager();
        public static async Task ResetData()
        {
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
