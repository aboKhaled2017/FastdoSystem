using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Fastdo.Repositories.Enums;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fastdo.backendsys;
using Fastdo.backendsys.Global;
using Fastdo.backendsys.Mappings;
using Fastdo.backendsys.Repositories;
using Fastdo.backendsys.Services;
using Fastdo.backendsys.Services.Auth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

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
            services.AddScoped<IAdminRepository, AdminRepository>();
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
            .AddCookie(Variables.AdminSchemaOfAdminSite, CookieBuilder =>
            {
                CookieBuilder.Cookie.Path = $"/{Variables.AdminPanelCookiePath}";
                CookieBuilder.LoginPath = "/AdminPanel/Auth/SignIn";
                CookieBuilder.AccessDeniedPath = "/AdminPanel/Auth/AccessDenied";
                CookieBuilder.Cookie.Name = "AdminCookie";
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

            //for admin panel
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
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
                opts.AddPolicy("AdminAreaAuthPolicy", policy =>
                {
                    policy.AuthenticationSchemes.Add(Variables.AdminSchemaOfAdminSite);
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole(Variables.adminer);
                });
                opts.AddPolicy(Variables.AdminPolicy, policy =>
                {
                    policy.RequireRole(Variables.adminer)
                          .RequireClaim(Variables.AdminClaimsTypes.AdminType,AdminType.Administrator)
                          .RequireAuthenticatedUser();
                });
                opts.AddPolicy(Variables.RepresentativePolicy, policy =>
                {
                    policy.RequireRole(Variables.adminer)
                          .RequireClaim(Variables.AdminClaimsTypes.AdminType, AdminType.Representative)
                          .RequireAuthenticatedUser();
                });
                opts.AddPolicy(Variables.FullControlOnSubAdminsPolicy, policy =>
                {
                    policy.RequireAssertion(p => {
                        var claimVal = p.User.Claims.FirstOrDefault(c => c.Type == Variables.AdminClaimsTypes.Previligs);
                        if (claimVal == null) return false;
                        return claimVal.Value.Split(",")
                                     .Contains(AdminPreviligs.FullControlOnSubAdmins);
                    });
                });
                opts.AddPolicy(Variables.ViewAnySubAdminPolicy, policy =>
                {
                    policy.RequireAssertion(p => {
                        var claimVal = p.User.Claims.FirstOrDefault(c => c.Type == Variables.AdminClaimsTypes.Previligs);
                        if (claimVal == null) return false;
                        return
                        claimVal.Value.Split(",")
                                     .Contains(AdminPreviligs.ViewAnySubAdmin)
                        ||
                        claimVal.Value.Split(",")
                                     .Contains(AdminPreviligs.FullControlOnSubAdmins);

                    });
                });
                opts.AddPolicy(Variables.CanAddSubAdminPolicy, policy =>
                { 
                    policy.RequireAssertion(p =>{
                        var claimVal = p.User.Claims.FirstOrDefault(c => c.Type == Variables.AdminClaimsTypes.Previligs);
                        if (claimVal == null) return false;
                        return claimVal.Value.Split(",")
                                     .Contains(AdminPreviligs.AddNewAdmin)
                        ||
                        claimVal.Value.Split(",")
                                     .Contains(AdminPreviligs.FullControlOnSubAdmins);
                    });
                });
                opts.AddPolicy(Variables.CanUpdateSubAdminPolicy, policy =>
                {
                    policy.RequireAssertion(p => {
                        var claimVal = p.User.Claims.FirstOrDefault(c => c.Type == Variables.AdminClaimsTypes.Previligs);
                        if (claimVal == null) return false;
                        return
                        claimVal.Value.Split(",")
                                     .Contains(AdminPreviligs.UpdateSubAdmin)
                        ||
                        claimVal.Value.Split(",")
                                     .Contains(AdminPreviligs.FullControlOnSubAdmins);
                    });
                });
                opts.AddPolicy(Variables.CanDeleteSubAdminPolicy, policy =>
                {
                    policy.RequireAssertion(p => {
                        var claimVal = p.User.Claims.FirstOrDefault(c => c.Type == Variables.AdminClaimsTypes.Previligs);
                        if (claimVal == null) return false;
                        return
                        claimVal.Value.Split(",")
                                     .Contains(AdminPreviligs.DeleteSubAdmin)
                        ||
                        claimVal.Value.Split(",")
                                     .Contains(AdminPreviligs.FullControlOnSubAdmins);
                    });
                });
                opts.AddPolicy(Variables.CanAddNewRepresentativePolicy, policy =>
                {
                    policy.RequireAssertion(p => {
                        var claimVal = p.User.Claims.FirstOrDefault(c => c.Type == Variables.AdminClaimsTypes.Previligs);
                        if (claimVal == null) return false;
                        return
                        claimVal.Value.Split(",")
                                     .Contains(AdminPreviligs.AddNewRepresentative)
                        ||
                        claimVal.Value.Split(",")
                                     .Contains(AdminPreviligs.FullControlOnSubAdmins);
                    });
                });

            });
            return services;
        }
    }
}
