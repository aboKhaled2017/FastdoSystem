using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using System_Back_End.Services;

namespace System_Back_End
{
    public static class Properties
    {
        private static readonly UserManager<AppUser> _userManager = RequestStaticServices.GetUserManager;
        private static readonly SysDbContext _context = RequestStaticServices.GetDbContext;
        public static readonly IConfiguration _configuration = RequestStaticServices.GetConfiguration;
        public static readonly ClaimsPrincipal User = RequestStaticServices.GetCurrentHttpContext.User;
        private static AppUser _getMainAdminUser()
        {
            string mainAdminUserName = _configuration
                .GetSection(Variables.AdminConfigSectionName)
                .GetValue<string>("userName");
            return _userManager.Users.FirstOrDefault(a => a.UserName == mainAdminUserName);
        }
        private static IConfigurationSection EmailConfingSection
        {
            get
            {
                return _configuration.GetSection(Variables.EmailSettingSectionName);
            }
        }
        public static EmailSetting EmailConfig
        {
            get
            {
                return new EmailSetting
                {
                    from = EmailConfingSection.GetValue<string>("from"),
                    password = EmailConfingSection.GetValue<string>("password"),
                    writeAsFile = EmailConfingSection.GetValue<bool>("writeAsFile")
                };
            }
            private set { }
        }
        public static string GetUsersImagesPath____(HttpContext httpContext)
        {
            return $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/images/Users/";
        }
        public static UserIdentifier UserIdentifier
        {
            get
            {
                return new UserIdentifier
                {
                    Email = User.Claims.FirstOrDefault(c=>c.Type=="Email").Value,
                    UserId = User.Claims.FirstOrDefault(c => c.Type == "UserId").Value,
                    Phone = User.Claims.FirstOrDefault(c => c.Type == "Phone").Value,
                    UserName = User.Claims.FirstOrDefault(c => c.Type == "UserName").Value,
                    Role=User.Claims.FirstOrDefault(c=>c.Type==ClaimTypes.Role).Value,

                };
            }
            set {}
        }
        public static UserType CurrentUserType
        {
            get
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
                return role == Variables.pharmacier
                    ? UserType.pharmacier
                    : UserType.stocker;
            }
            set { }
        }
        
    }
}
