using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Core.Models;
using System.Threading.Tasks;
using Fastdo.backendsys.Models;
using Fastdo.backendsys.Controllers.Pharmacies;
using Fastdo.Core.Enums;
using System.Collections;

namespace Fastdo.backendsys.Repositories
{
    public class PharmacyRepository:Repository<Pharmacy>,IPharmacyRepository
    {
        public PharmacyRepository(SysDbContext context) : base(context)
        {
        }
        public async Task<PagedList<Get_PageOf_Pharmacies_ADMModel>> Get_PageOf_PharmacyModels_ADM(PharmaciesResourceParameters _params)
        {

            var sourceData = _context.Pharmacies
            .OrderBy(d => d.Name)
            .Select(p => new Get_PageOf_Pharmacies_ADMModel
            {
                Id = p.Id,
                MgrName = p.MgrName,
                Email=p.User.Email,
                Name = p.Name,
                OwnerName = p.OwnerName,
                PersPhone = p.PersPhone,
                LandlinePhone = p.LandlinePhone,
                LicenseImgSrc = p.LicenseImgSrc,
                CommercialRegImgSrc = p.CommercialRegImgSrc,
                Status = p.Status,
                Address = $"{p.Area.SuperArea.Name}/{p.Area.Name}",
                AreaId = p.AreaId,
                joinedStocksCount = p.GoinedStocks.Count,
                lzDrugsCount = p.LzDrugs.Count,
                requestedDrugsCount = p.RequestedLzDrugs.Count
            });
            if (!string.IsNullOrEmpty(_params.S))
            {
                var searchQueryForWhereClause = _params.S.Trim().ToLowerInvariant();
                sourceData = sourceData
                     .Where(d => d.Name.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }
            if (_params.Status != null)
            {
                sourceData = sourceData
                     .Where(p=>p.Status==_params.Status);
            }
            return await PagedList<Get_PageOf_Pharmacies_ADMModel>.CreateAsync(sourceData, _params);
        }

        public async Task<Get_PageOf_Pharmacies_ADMModel> Get_PharmacyModel_ADM(string id)
        {
            return await _context.Pharmacies
                .Where(p=>p.Id==id)
                .Select(p => new Get_PageOf_Pharmacies_ADMModel
            {
                Id=p.Id,
                Email=p.User.Email,
                MgrName=p.MgrName,
                Name=p.Name,
                OwnerName=p.OwnerName,
                PersPhone=p.PersPhone,
                LandlinePhone=p.LandlinePhone,
                LicenseImgSrc=p.LicenseImgSrc,
                CommercialRegImgSrc=p.CommercialRegImgSrc,
                Status=p.Status,
                Address = $"{p.Area.SuperArea.Name}/{p.Area.Name}",
                AreaId =p.AreaId,
                joinedStocksCount=p.GoinedStocks.Count,
                lzDrugsCount=p.LzDrugs.Count,
                requestedDrugsCount=p.RequestedLzDrugs.Count               
                })
               .SingleOrDefaultAsync();
        }
       
        public async Task<bool> UpdateAsync(Pharmacy pharmacy)
        {
            _context.Entry(pharmacy).State = EntityState.Modified;
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task Delete(Pharmacy pharm)
        {                   
            _context.LzDrugRequests.RemoveRange(
                _context.LzDrugRequests.Where(r => pharm.Id == r.PharmacyId || pharm.Id == r.LzDrug.PharmacyId)
            );
            await _context.SaveChangesAsync();            
            _context.Users.Remove(await _context.Users.FindAsync(pharm.Id));
        }
        public void UpdatePhone(Pharmacy pharmacy)
        {
            UpdateFields(pharmacy, prop => prop.PersPhone);
        }
        public void UpdateName(Pharmacy pharmacy)
        {
            UpdateFields(pharmacy, prop => prop.Name);
        }
        public void UpdateContacts(Pharmacy pharmacy)
        {
            UpdateFields(pharmacy,
                prop => prop.LandlinePhone,
                prop => prop.Address);
        }
        public async Task<bool> Patch_Apdate_ByAdmin(Pharmacy pharm)
        {
            return await UpdateFieldsAsync_And_Save(pharm,prop => prop.Status);
        }

        //will be moved from here/not suitable
        public async Task<List<ShowSentRequetsToStockByPharmacyModel>> GetSentRequestsToStocks()
        {
          return await _context.PharmaciesInStocks
                 .Where(r => r.PharmacyId == UserId && r.PharmacyReqStatus!=PharmacyRequestStatus.Accepted)
                .Select(r => new ShowSentRequetsToStockByPharmacyModel
                {
                    StockId = r.StockId,
                    Seen = r.Seen,
                    Status = r.PharmacyReqStatus,
                    Address = $"{r.Stock.Area.SuperArea.Name}/{r.Stock.Area.Name}",
                    AddressInDetails=r.Stock.Address,
                    LandLinePhone=r.Stock.LandlinePhone,
                    Name=r.Stock.Name,
                    PersPhone=r.Stock.PersPhone
                }).ToListAsync();
        }

        //will be moved from here/not suitable
        public async Task<IEnumerable<ShowJoinedStocksOfPharmaModel>> GetUserJoinedStocks()
        {
            return await _context.PharmaciesInStocks
                .Where(s => s.PharmacyId == UserId && s.PharmacyReqStatus == PharmacyRequestStatus.Accepted)
                .Select(s => new ShowJoinedStocksOfPharmaModel{ 
                 StockId=s.StockId,
                 Address = $"{s.Stock.Area.SuperArea.Name} / {s.Stock.Area.Name}",
                 AddressInDetails=s.Stock.Address,
                 Name =s.Stock.Name,
                 PhoneNumber=s.Stock.PersPhone,
                 LandeLinePhone=s.Stock.LandlinePhone
                })
                .ToListAsync();
        }

        //will be moved from here/not suitable
        public async Task<List<List<string>>> GetPharmaClassesOfJoinedStocks(string pharmaId)
        {
                 return await _context.PharmaciesInStocks
                .Where(r => r.PharmacyId == pharmaId)
                .Select(r => new List<string> { r.StockId, r.Pharmacy.StocksClasses.SingleOrDefault(s=>s.StockClass.StockId==r.StockId).StockClass.ClassName??string.Empty})
                .ToListAsync();
        }
    }
}
