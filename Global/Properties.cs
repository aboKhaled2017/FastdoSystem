using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Core.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using Fastdo.backendsys.Global;
using Fastdo.backendsys.Services;

namespace Fastdo.backendsys
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
                    UserName = MainAdministratorConfigSection.GetValue<string>("userName"),
                    Name = MainAdministratorConfigSection.GetValue<string>("name"),
                    PhoneNumber = MainAdministratorConfigSection.GetValue<string>("phone"),
                    Password = MainAdministratorConfigSection.GetValue<string>("password")
                };
            }
        }

    }
}
