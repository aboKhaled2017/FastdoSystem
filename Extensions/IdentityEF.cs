using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Fastdo.Core.Enums;
using System.Linq;
using Fastdo.Core.Models;
using System.Threading.Tasks;
using Fastdo.backendsys;
using Fastdo.backendsys.Global;

namespace Microsoft.AspNetCore.Identity
{
    public static class IdentityEF
    {
        public async static Task<bool> AnyEmailSync(this UserManager<AppUser> userManager, string email)
        {
            return await userManager.Users.AnyAsync(u => u.Email == email);
        }
        public async static Task<bool> AnyPhoneSync(this UserManager<AppUser> userManager, string phone)
        {
            return await userManager.Users.AnyAsync(u => u.PhoneNumber == phone);
        }
        public static async Task _addRoles(this RoleManager<IdentityRole> _roleManager, List<string> roles)
        {
            foreach (var name in roles)
            {
                if (!await _roleManager.RoleExistsAsync(name))
                {
                    var role = new IdentityRole(name);
                    await _roleManager.CreateAsync(role);
                }
            }
        }
        public async static Task<bool> UserIdentityExists(this UserManager<AppUser> userManager, AppUser user, string password)
        {
            return user != null &&
                await userManager.CheckPasswordAsync(user, password);
        }
        public async static Task<bool> UserIdentityExists(this UserManager<AppUser> userManager, AppUser user, string password,UserType userType)
        {
            string role = userType == UserType.pharmacier
                ? Variables.pharmacier
                : Variables.stocker;
            return user != null &&
                await userManager.IsInRoleAsync(user, role) &&
                await userManager.CheckPasswordAsync(user, password);
        }
        public async static Task<bool> UserIdentityExists(this UserManager<AppUser> userManager, AppUser user, string password,string adminType)
        {
            return user != null &&
                await userManager.IsInRoleAsync(user, Variables.adminer) &&
                (await userManager.GetClaimsAsync(user))
                .Any(c=>c.Type==Variables.AdminClaimsTypes.AdminType && c.Value==adminType) &&
                await userManager.CheckPasswordAsync(user, password);
        }
    }
}
