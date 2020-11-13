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
using Fastdo.Core.Enums;
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
using NSwag.Generation.Processors.Security;
using NSwag;
using Fastdo.backendsys.Graphql;
using GraphQL.Types;
using GraphQL;
using GraphQL.Server;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Fastdo.backendsys.Configurations;

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
            services.AddTransient<StockUserServices>();
            services.AddSingleton<HandlingProofImgsServices>();
            services.AddSingleton<IEmailSender,EmailSender>();
            services.AddTransient<StkDrugsReportFromExcelService>();
            services.AddSingleton<IActionContextAccessor,ActionContextAccessor>();
            services.AddScoped<IUrlHelper,UrlHelper>(implementationFactory=> {
                var actionContext = implementationFactory.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });
            services.AddScoped<IExecuterDelayer, ExecuterDelayer>();
            return services;
        }
        public static IServiceCollection ConfigureIdentityServerServices(IServiceCollection services)
        {
            var builder = services.AddIdentityServer()
                .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
                .AddInMemoryApiResources(IdentityServerConfig.GetApis())
                .AddInMemoryClients(IdentityServerConfig.GetClients());
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
            services.AddScoped<IStkDrugsRepository, StkDrugsRepository>();

            services.AddScoped<IStkDrugsRepository, StkDrugsRepository>();
            services.AddScoped<IStockWithClassRepository, StockWithClassRepository>();
            services.AddScoped<IPharmacyInStkRepository, PharmacyInStkRepository>();
            services.AddScoped<IPharmacyInStkClassRepository, PharmacyInStkClassRepository>();
            services.AddScoped<IStkDrugPackgesReqsRepository, StkDrugPackagesReqsRepository>();
            services.AddScoped<IStockInPackagesReqsRepository, StockInPackagesReqsRepository>();
            services.AddScoped<IStkDrgInPackagesReqsRepository, StkDrgInPackagesReqs>();
            return services;
        }
        public static IServiceCollection _AddGraphQlServices(this IServiceCollection services)
        {
            /*services.AddScoped<FastdoQuery>();
            services.AddTransient<StockType>();
            services.AddTransient<StkDrugType>();*/
            services.AddScoped<IDependencyResolver>(s => new FuncDependencyResolver(s.GetRequiredService));
            services.AddScoped<FastdoSchema>();
            services.AddGraphQL(o =>
            {
                o.ExposeExceptions = false;
            })
                .AddGraphTypes(ServiceLifetime.Scoped)
                .AddDataLoader();
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            return services;
        }
        public static IServiceCollection _AddSwaggr(this IServiceCollection services)
        {
            services
               .AddOpenApiDocument(c => {
                   c.Title = "Fastdo Api v1";
                   c.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                   {
                       Type = OpenApiSecuritySchemeType.ApiKey,
                       Name = "Authorization",
                       In = OpenApiSecurityApiKeyLocation.Header,
                       Description = "Type into the textbox: Bearer {your JWT token}."
                   });

                   c.OperationProcessors.Add(
                       new AspNetCoreOperationSecurityScopeProcessor("JWT"));
               });
            /* services.AddSwaggerGen(c =>
             {
                 c.SwaggerDoc("v1", new Info
                 {
                     Title = "Fastdo API",
                     Description="Fisrt version of Fastdo API",
                     Version = "v1"
                 });

                 /*var security = new Dictionary<string, IEnumerable<string>>
                 {
                     {"Bearer",new string[0] }
                 };
                 c.AddSecurityDefinition("Bearer",
                     new ApiKeyScheme
                     {
                         In = "header",
                         Description = "Please enter into field the word 'Bearer' following by space and JWT",
                         Name = "Authorization",
                         Type = "apiKey"
                     });
                             c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                     { "Bearer", Enumerable.Empty<string>() },
                 });
             });*/
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
                CookieBuilder.LogoutPath = "/AdminPanel/Auth/LogOut";
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
                #region role based policies
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
                opts.AddPolicy(Variables.AdministratorPolicy, policy =>
                {
                    policy.RequireRole(Variables.adminer)
                          .RequireClaim(Variables.AdminClaimsTypes.AdminType, AdminType.Administrator)
                          .RequireAuthenticatedUser();
                });

                #endregion

                #region admin  area policies
                opts.AddPolicy(Variables.AdminPanelPolicies.AdminPanelAuthPolicy, policy =>
                {
                    policy.AuthenticationSchemes.Add(Variables.AdminSchemaOfAdminSite);
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole(Variables.adminer);
                });

                
                #endregion

                #region admin api policies
                opts.AddPolicy(Variables.AdminPolicies.HaveFullControlPolicy, policy =>
                {
                    policy.RequireAssertion(p => {
                        var claimVal = p.User.Claims.FirstOrDefault(c => c.Type == Variables.AdminClaimsTypes.Priviligs);
                        if (claimVal == null) return false;
                        return claimVal.Value.Split(",")
                                     .Contains(AdminerPreviligs.HaveFullControl.ToString());
                    });
                });
                opts.AddPolicy(Variables.AdminPolicies.ControlOnAdministratorsPagePolicy, policy =>
                {
                    policy.RequireAssertion(p => {
                        var claimVal = p.User.Claims.FirstOrDefault(c => c.Type == Variables.AdminClaimsTypes.Priviligs);
                        if (claimVal == null) return false;
                        return
                        claimVal.Value.Split(",")
                                     .Contains(AdminerPreviligs.HaveControlOnAdminersPage.ToString())
                        ||
                        claimVal.Value.Split(",")
                                     .Contains(AdminerPreviligs.HaveFullControl.ToString());

                    });
                });
                opts.AddPolicy(Variables.AdminPolicies.ControlOnDrugsRequestsPagePolicy, policy =>
                {
                    policy.RequireAssertion(p => {
                        var claimVal = p.User.Claims.FirstOrDefault(c => c.Type == Variables.AdminClaimsTypes.Priviligs);
                        if (claimVal == null) return false;
                        return
                        claimVal.Value.Split(",")
                                     .Contains(AdminerPreviligs.HaveControlOnDrugsREquestsPage.ToString())
                        ||
                        claimVal.Value.Split(",")
                                     .Contains(AdminerPreviligs.HaveFullControl.ToString());

                    });
                });
                opts.AddPolicy(Variables.AdminPolicies.ControlOnPharmaciesPagePolicy, policy =>
                {
                    policy.RequireAssertion(p => {
                        var claimVal = p.User.Claims.FirstOrDefault(c => c.Type == Variables.AdminClaimsTypes.Priviligs);
                        if (claimVal == null) return false;
                        return
                        claimVal.Value.Split(",")
                                     .Contains(AdminerPreviligs.HaveControlOnPharmaciesPage.ToString())
                        ||
                        claimVal.Value.Split(",")
                                     .Contains(AdminerPreviligs.HaveFullControl.ToString());

                    });
                });
                opts.AddPolicy(Variables.AdminPolicies.ControlOnStocksPagePolicy, policy =>
                {
                    policy.RequireAssertion(p => {
                        var claimVal = p.User.Claims.FirstOrDefault(c => c.Type == Variables.AdminClaimsTypes.Priviligs);
                        if (claimVal == null) return false;
                        return
                        claimVal.Value.Split(",")
                                     .Contains(AdminerPreviligs.HaveControlOnStocksPage.ToString())
                        ||
                        claimVal.Value.Split(",")
                                     .Contains(AdminerPreviligs.HaveFullControl.ToString());

                    });
                });
                opts.AddPolicy(Variables.AdminPolicies.ControlOnVStockPagePolicy, policy =>
                {
                    policy.RequireAssertion(p => {
                        var claimVal = p.User.Claims.FirstOrDefault(c => c.Type == Variables.AdminClaimsTypes.Priviligs);
                        if (claimVal == null) return false;
                        return
                        claimVal.Value.Split(",")
                                     .Contains(AdminerPreviligs.HaveControlOnVStockPage.ToString())
                        ||
                        claimVal.Value.Split(",")
                                     .Contains(AdminerPreviligs.HaveFullControl.ToString());

                    });
                });
                #endregion
            });
            return services;
        }
    }
}
