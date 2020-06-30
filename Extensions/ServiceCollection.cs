using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System_Back_End.Mappings;
using System_Back_End.Repositories;
using System_Back_End.Services;
using System_Back_End.Services.Auth;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollection
    {
        public static void _AddAutoMapper(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //services.AddAutoMapper(new[] {typeof(MappingProfile).Assembly});
        }
        public static IServiceCollection _AddSystemServices(this IServiceCollection services)
        {
            services.AddTransient<JWThandlerService>();          
            services.AddTransient<AccountService>();
            services.AddTransient<TransactionService>();
            services.AddSingleton<HandlingProofImgsServices>();
            services.AddSingleton<IEmailSender,EmailSender>();
            return services;
        }
        public static IServiceCollection _AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<PharmacyRepository>();
            services.AddTransient<StockRepository>();
            return services;
        }
        public static IServiceCollection _AddSystemAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(options =>
           {
               var JWTSection = RequestStaticServices.GetConfiguration.GetSection("JWT");
               options.SaveToken = true;
               options.RequireHttpsMetadata = false;//disabled only in developement
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
               {
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidIssuer = JWTSection.GetValue<string>("issuer"),
                   ValidAudience = JWTSection.GetValue<string>("audience"),
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTSection.GetValue<string>("signingKey")))
               };
           });
            return services;
        }
    }
}
