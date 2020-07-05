using System;
using System.Collections.Generic;
using System.Linq;
using System.Models;
using System.Threading.Tasks;
using System_Back_End.Models;

namespace System_Back_End.Repositories
{
    public class LzDrg_Search_Repository : MainRepository, ILzDrg_Search_Repository
    {
        public LzDrg_Search_Repository(SysDbContext context) : base(context)
        {
        }
        public async Task<PagedList<LzDrugCard_Info_BM>> Get_All_LzDrug_Cards_BMs(LzDrg_Card_Info_BM_ResourceParameters _params)
        {
            var items = _context.LzDrugs
                .Where(d=>d.PharmacyId!=UserId)
                .Select(d => new LzDrugCard_Info_BM { 
            Id=d.Id,
            Name=d.Name,
            Desc=d.Desc,
            Discount=d.Discount,
            PharmacyId=d.PharmacyId,
            Price=d.Price,
            PriceType=d.PriceType,
            Quantity=d.Quantity,
            Type=d.Type,
            UnitType=d.UnitType,
            ValideDate=d.ValideDate,
            PharmName=d.Pharmacy.Name,
            PharmLocation=d.Pharmacy.Area.SuperArea.Name +"/"+d.Pharmacy.Area.Name,
            RequestsCount=d.RequestingPharms.Count,
            IsMadeRequest= (d.RequestingPharms.Count > 0 && d.RequestingPharms.Any(r => r.PharmacyId == UserId)),
            Status =
              (d.RequestingPharms.Count>0 && d.RequestingPharms.Any(r=>r.PharmacyId==UserId))
            ?d.RequestingPharms.FirstOrDefault(r=>r.PharmacyId==UserId).Status
            :0
            });
            return await PagedList<LzDrugCard_Info_BM>.CreateAsync(items, _params);
        }
    }
}
