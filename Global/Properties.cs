using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using System_Back_End.Global;
using System_Back_End.Services;

namespace System_Back_End
{
    public static class Properties
    {
        
        private static IConfigurationSection EmailConfingSection
        {
            get
            {
                return RequestStaticServices.GetConfiguration().GetSection(Variables.EmailSettingSectionName);
            }
        }
        private static IConfigurationSection MainAdministratorConfigSection
        {
            get
            {
                return RequestStaticServices.GetConfiguration().GetSection(Variables.AdminerConfigSectionName);
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
        }
        public static AdministratorInfo MainAdministratorInfo
        {
            get
            {
                return new AdministratorInfo
                {
                    Email = EmailConfingSection.GetValue<string>("email"),
                    Password = EmailConfingSection.GetValue<string>("password")
                };
            }
        }

    }
}
