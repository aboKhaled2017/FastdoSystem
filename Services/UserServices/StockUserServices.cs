using Fastdo.backendsys.Models;
using Fastdo.Core.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Services
{
    public class StockUserServices
    {
        private readonly UserManager<AppUser> _userManager;
        private ClaimsPrincipal _User { get { return RequestStaticServices.GetCurrentHttpContext().User; } }
        public StockUserServices(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public List<StockClassWithPharmaCountsModel> GetStockClassesForPharmas(ClaimsPrincipal User)
        {
            var classesStr =User.Claims.SingleOrDefault(c => c.Type == Variables.StockUserClaimsTypes.PharmasClasses)?.Value??null;
            return string.IsNullOrEmpty(classesStr)
                ? new List<StockClassWithPharmaCountsModel>()
                : JsonConvert.DeserializeObject<List<StockClassWithPharmaCountsModel>>(classesStr);
                
        }
        public bool IsStockHasClass(Guid forClassId,ClaimsPrincipal User)
        {
            string classes = User.Claims.SingleOrDefault(t => t.Type == Variables.StockUserClaimsTypes.PharmasClasses)?.Value ?? null;
            if (string.IsNullOrEmpty(classes)) return false;
            return (JsonConvert.DeserializeObject<List<StockClassWithPharmaCountsModel>>(classes).Any(c=>c.Id== forClassId));
        }
        public bool IsStockHasClassName(string forClass, ClaimsPrincipal User)
        {
            string classes = User.Claims.SingleOrDefault(t => t.Type == Variables.StockUserClaimsTypes.PharmasClasses)?.Value ?? null;
            if (string.IsNullOrEmpty(classes)) return false;
            return (JsonConvert.DeserializeObject<List<StockClassWithPharmaCountsModel>>(classes).Any(c => c.Name == forClass));
        }
        public bool IsStockHasSinglePharmaClasses(ClaimsPrincipal User)
        {
            string classes = User.Claims.SingleOrDefault(t => t.Type == Variables.StockUserClaimsTypes.PharmasClasses)?.Value ?? null;
            return classes == null ? true : JsonConvert.DeserializeObject<List<StockClassWithPharmaCountsModel>>(classes).Count==1;
        }
    }
}
