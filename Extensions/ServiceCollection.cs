using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System_Back_End;
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
            services.AddTransient<IpropertyMappingService,PropertyMappingService>();
            services.AddTransient<JWThandlerService>();          
            services.AddTransient<AccountService>();
            services.AddTransient<TransactionService>();
            services.AddSingleton<HandlingProofImgsServices>();
            services.AddSingleton<IEmailSender,EmailSender>();
            services.AddSingleton<IActionContextAccessor,ActionContextAccessor>();
            services.AddScoped<IUrlHelper,UrlHelper>(implementationFactory=> {
                var actionContext = implementationFactory.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });
            services.AddScoped<IExecuterDelayer, ExecuterDelayer>();
            return services;
        }
        public static IServiceCollection _AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IPharmacyRepository,PharmacyRepository>();
            services.AddScoped<IStockRepository,StockRepository>();
            services.AddScoped<ILzDrugRepository,LzDrugRepository>();
            services.AddScoped<ILzDrgRequestsRepository,LzDrgRequestsRepository>();
            services.AddScoped<IComplainsRepository,ComplainsRepository>();
            services.AddScoped<ILzDrg_Search_Repository,LzDrg_Search_Repository>();
            services.AddScoped<IAreaRepository, AreaRepository>();
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
               var JWTSection = RequestStaticServices.GetConfiguration().GetSection("JWT");
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
        public static IServiceCollection _AddSystemAuthorizations(this IServiceCollection services)
        {
            services.AddAuthorization(opts => {
                opts.AddPolicy(Variables.PharmacyPolicy, policy => {
                    policy.RequireRole(Variables.pharmacier)
                           .RequireAuthenticatedUser();
                });
                opts.AddPolicy(Variables.StockPolicy, policy => {
                    policy.RequireRole(Variables.stocker)
                           .RequireAuthenticatedUser();
                });
                opts.AddPolicy(Variables.Stock_Or_PharmacyPolicy, policy =>
                {
                    policy.RequireRole(new List<string> { Variables.pharmacier, Variables.stocker })
                          .RequireAuthenticatedUser();
                });
            });
            return services;
        }
    }
}
