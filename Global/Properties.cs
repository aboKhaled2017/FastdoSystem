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
        
        private static AppUser _getMainAdminUser()
        {
            string mainAdminUserName = RequestStaticServices.GetConfiguration()
                .GetSection(Variables.AdminConfigSectionName)
                .GetValue<string>("userName");
            return RequestStaticServices.GetUserManager().Users.FirstOrDefault(a => a.UserName == mainAdminUserName);
        }
        private static IConfigurationSection EmailConfingSection
        {
            get
            {
                return RequestStaticServices.GetConfiguration().GetSection(Variables.EmailSettingSectionName);
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
        
    }
}
