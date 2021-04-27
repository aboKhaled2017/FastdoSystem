

using Fastdo.Core.Services;
using FastDo.Common;
using Fastdo.CommonGlobal;
using Microsoft.Extensions.Configuration;

namespace Fastdo.Core
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
