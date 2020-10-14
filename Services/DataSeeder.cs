using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fastdo.Core.Models;
using Microsoft.AspNetCore.Identity;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using AutoMapper;
using Fastdo.backendsys.InitSeeds.Helpers;
using System.Security.Claims;
using Fastdo.backendsys.Global;

namespace Fastdo.backendsys.Services
{
    public static class DataSeeder
    {
        private static readonly SysDbContext context = RequestStaticServices.GetDbContext();
        private static readonly UserManager<AppUser> _userManager = RequestStaticServices.GetUserManager();
        private static readonly IHostingEnvironment _env = RequestStaticServices.GetHostingEnv();
        private static readonly IMapper _mapper = RequestStaticServices.GetMapper();
        public static async Task SeedBasicData()
        {
            if(!context.Areas.Any())
            await SeedAreas();
            await SeedDefaultAdminstrator();
        }
        public static async Task SeedDefaultData()
        {
           await SeedPharmacies();
           await SeedStocks();
           await SeedLzDrugs();
           await SeedStkDrugs();
        }
        public static async Task SeedDefaultAdminstrator()
        {
            var user = await _userManager.FindByNameAsync(Properties.MainAdministratorInfo.UserName);
            if (user != null)
                return;            
            user = new AppUser
            {
                UserName = Properties.MainAdministratorInfo.UserName,
                PhoneNumber= Properties.MainAdministratorInfo.PhoneNumber
            };
            var res = await _userManager.CreateAsync(user, Properties.MainAdministratorInfo.Password);
            if (!res.Succeeded)
                throw new Exception("cannot add the default administrator");
            await _userManager.AddToRoleAsync(user, Variables.adminer);
            await _userManager.AddClaimsAsync(user, new List<Claim> {
              new Claim(Variables.AdminClaimsTypes.AdminType,AdminType.Administrator),
              new Claim(Variables.AdminClaimsTypes.Priviligs,AdminerPreviligs.HaveFullControl.ToString())
            });
            var admin = new Admin
            {
                Name = Properties.MainAdministratorInfo.Name,
                User = user
            };
             context.Admins.Add(admin);
            if (await context.SaveChangesAsync() < 1)
                await _userManager.DeleteAsync(user);
        }
        private static async Task SeedAreas()
        {
            context.Areas.AddRange(new List<Area>
            {
                new Area
                {
                    Id=1,
                    Name="سوهاج",
                },
                new Area
                {
                    Id=2,
                    Name="دار السلام",
                    SuperAreaId=1
                },
                new Area
                {
                    Id=3,
                    Name="اخميم",
                    SuperAreaId=1
                },
                new Area
                {
                    Id=4,
                    Name="جهينة",
                    SuperAreaId=1
                },
                new Area
                {
                    Id=5,
                    Name="البلينا",
                    SuperAreaId=1
                },
                new Area
                {
                    Id=6,
                    Name="المحافظة",
                    SuperAreaId=1
                },
                new Area
                {
                    Id=7,
                    Name="ساقلتة",
                    SuperAreaId=1
                },
                new Area
                {
                    Id=8,
                    Name="طهطا",
                    SuperAreaId=1
                },
                new Area
                {
                    Id=9,
                    Name="المراغة",
                    SuperAreaId=1
                },
                new Area
                {
                    Id=10,
                    Name="طما",
                    SuperAreaId=1
                },
                new Area
                {
                    Id=11,
                    Name="جرجا",
                    SuperAreaId=1
                },
                new Area
                {
                    Id=12,
                    Name="اسيوط",
                },
                new Area
                {
                    Id=13,
                    Name="ابوتيج",
                    SuperAreaId=12
                },
                new Area
                {
                    Id=14,
                    Name="صدفة",
                    SuperAreaId=12
                },
                new Area
                {
                    Id=15,
                    Name="البدارى",
                    SuperAreaId=12
                },
                new Area
                {
                    Id=16,
                    Name="المحافظة",
                    SuperAreaId=12
                },
            });
           await context.SaveChangesAsync();
        }
        private static async Task SeedPharmacies()
        {
            var fileJsonPath = Path.Combine(_env.ContentRootPath, "InitSeeds", "Pharmacies.json");
            var jsonData = File.ReadAllText(fileJsonPath);
            List<PharmacierObjectSeeder> Pharmacies =
                JsonConvert.DeserializeObject<List<PharmacierObjectSeeder>>(jsonData);
            if (!context.Pharmacies.Any())
            {
                Pharmacies.ForEach(ph =>
                {
                    var newUser = new AppUser
                    {
                        Id=ph.Id,
                        Email=ph.Email,
                        UserName =ph.UserName,     
                        PhoneNumber=ph.PersPhone
                    };
                    var result=_userManager.CreateAsync(newUser, ph.Password);
                    result.Wait();
                    if(result.IsCompletedSuccessfully)
                    _userManager.AddToRoleAsync(newUser, Variables.pharmacier).Wait();
                });
                context.Pharmacies.AddRange(Pharmacies);
                await context.SaveChangesAsync();
            }
        }
        private static async Task SeedStocks()
        {
            var fileJsonPath = Path.Combine(_env.ContentRootPath, "InitSeeds", "Stocks.json");
            var jsonData = File.ReadAllText(fileJsonPath);
            List<StockerObjectSeeder> Stocks =
                JsonConvert.DeserializeObject<List<StockerObjectSeeder>>(jsonData);
            if (!context.Stocks.Any())
            {
                Stocks.ForEach(stk =>
                {
                    var newUser = new AppUser
                    {
                        Id=stk.Id,
                        Email=stk.Email,
                        UserName = stk.UserName,
                        PhoneNumber = stk.PersPhone
                    };
                    var result=_userManager.CreateAsync(newUser, stk.Password);
                    result.Wait();
                    if(result.IsCompletedSuccessfully)
                    _userManager.AddToRoleAsync(newUser, Variables.stocker).Wait();
                });
                context.Stocks.AddRange(Stocks);
                await context.SaveChangesAsync();
            }
        }
        private static async Task SeedLzDrugs()
        {
            var fileJsonPath = Path.Combine(_env.ContentRootPath, "InitSeeds", "LzDrugs.json");
            var jsonData = File.ReadAllText(fileJsonPath);
            List<LzDrug> lzDrugs =
                JsonConvert.DeserializeObject<List<LzDrug>>(jsonData);
            if (!context.LzDrugs.Any())
            {
                context.LzDrugs.AddRange(lzDrugs);
                await context.SaveChangesAsync();
            }
        }
        private static async Task SeedStkDrugs()
        {
            var fileJsonPath = Path.Combine(_env.ContentRootPath, "InitSeeds", "StkDrugs.json");
            var jsonData = File.ReadAllText(fileJsonPath);
            List<StkDrug> StkDrugs =
                JsonConvert.DeserializeObject<List<StkDrug>>(jsonData);
            if (!context.StkDrugs.Any())
            {
                context.StkDrugs.AddRange(StkDrugs);
                await context.SaveChangesAsync();
            }
        }
    }
}
