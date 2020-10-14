using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NHibernate.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Core.Models;
using System.Threading.Tasks;
using Fastdo.backendsys.Mappings;

namespace Fastdo.backendsys.Services
{
    public static class RequestStaticServices
    {
        private static IHttpContextAccessor _httpContextAccessor { get; set; }
        private static IServiceProvider _serviceProvider { get; set; }
        private static IServiceScope _serviceScope { get; set; }
        public static void Init(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        }
        public static HttpContext GetCurrentHttpContext()
        {
           return _serviceScope.ServiceProvider.GetService<IHttpContextAccessor>().HttpContext; 
        }
        public static SysDbContext GetDbContext()
        {
            return _serviceScope.ServiceProvider.GetService<SysDbContext>();
        }
        public static IHostingEnvironment GetHostingEnv()
        {
            return _serviceScope.ServiceProvider.GetService<IHostingEnvironment>(); 
        }
        public static ILogger<T> GetLogger<T>()
        {
            return _serviceScope.ServiceProvider.GetService<ILogger<T>>();
        }
        public static UserManager<AppUser> GetUserManager()
        {
             return _serviceScope.ServiceProvider.GetService<UserManager<AppUser>>(); 
        }
        public static RoleManager<IdentityRole> GetRoleManager()
        {
             return _serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>(); 
        }       
        public static IConfiguration GetConfiguration()
        {
            return _serviceScope.ServiceProvider.GetService<IConfiguration>(); 
        }
        public static TransactionHelper GetTransactionHelper()
        {
            return _serviceScope.ServiceProvider.GetService<TransactionHelper>();          
        }
        public static TransactionService GetTransactionService()
        {
            return _serviceScope.ServiceProvider.GetService<TransactionService>();
        }
        public static IMapper GetMapper ()
        {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new MappingProfile());
                });

                return mappingConfig.CreateMapper();            
        }
    }
}
