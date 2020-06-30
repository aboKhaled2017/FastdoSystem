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
using System.Models;
using System.Threading.Tasks;
using System_Back_End.Mappings;

namespace System_Back_End.Services
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
        public static HttpContext GetCurrentHttpContext
        {
            get { return _serviceScope.ServiceProvider.GetService<IHttpContextAccessor>().HttpContext; }
            private set { }
        }
        public static SysDbContext GetDbContext
        {
            get {return _serviceScope.ServiceProvider.GetService<SysDbContext>();}
        }
        public static IHostingEnvironment GetHostingEnv
        {
            get { return _serviceScope.ServiceProvider.GetService<IHostingEnvironment>(); }
            private set { }
        }
        public static ILogger<T> GetLogger<T>()
        {
            return _serviceScope.ServiceProvider.GetService<ILogger<T>>();
        }
        public static UserManager<AppUser> GetUserManager
        {
            get { return _serviceScope.ServiceProvider.GetService<UserManager<AppUser>>(); }
            private set { }
}
        public static RoleManager<IdentityRole> GetRoleManager
        {
            get { return _serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>(); }
            private set { }
        }       
        public static IConfiguration GetConfiguration
        {
            get { return _serviceScope.ServiceProvider.GetService<IConfiguration>(); }
            private set { }
        }
        public static TransactionHelper GetTransactionHelper
        {
            get { return _serviceScope.ServiceProvider.GetService<TransactionHelper>(); }
            private set{ }
        }
        public static IMapper GetMapper {
            get {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new MappingProfile());
                });

                return mappingConfig.CreateMapper();
            } 
        }
    }
}
