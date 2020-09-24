using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Repositories.Models;
using System.Threading.Tasks;
using Fastdo.backendsys.Models;
using Fastdo.backendsys.Controllers.Stocks.Models;
using Fastdo.backendsys.Controllers.Stocks;

namespace Fastdo.backendsys.Repositories
{
    public class StockRepository:MainRepository,IStockRepository
    {
        public StockRepository(SysDbContext context) : base(context)
        {
        }

        public async Task<bool> AddAsync(Stock stock)
        {
            _context.Stocks.Add(stock);
            return (await _context.SaveChangesAsync()) > 0;
        }
        public IQueryable GetAllAsync()
        {
            return _context.Stocks;
        }
        public async Task<PagedList<GetPageOfSearchedStocks>> GetPageOfSearchedStocks(StockSearchResourceParameters _params)
        {
            var originalData = _context.Stocks
            .OrderBy(d => d.Name)
            .Where(s => s.User.EmailConfirmed && !s.GoinToPharmacies.Any(g=>g.PharmacyId==UserId));
            
            if (!string.IsNullOrEmpty(_params.S))
            {
                var searchQueryForWhereClause = _params.S.Trim().ToLowerInvariant();
                originalData = originalData
                     .Where(d => d.Name.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }

            if (_params.AreaIds != null && _params.AreaIds.Count() != 0)
            {
                originalData = originalData
                .Where(d => _params.AreaIds.Any(aid => aid == d.AreaId));
            }
            else if (_params.CityIds != null && _params.CityIds.Count() != 0)
            {
                originalData = originalData
                .Where(d => _params.CityIds.Any(cid => cid == d.Area.SuperAreaId));
            }

            var selectedData= originalData
                .Select(p => new GetPageOfSearchedStocks
                {
                    Id = p.Id,
                    Name = p.Name,
                    PersPhone = p.PersPhone,
                    LandlinePhone = p.LandlinePhone,
                    AddressInDetails = p.Address,
                    Address = $"{p.Area.SuperArea.Name}/{p.Area.Name}",
                    AreaId = p.AreaId,
                    joinedPharmesCount = p.GoinToPharmacies.Count,
                    drugsCount = p.SDrugs.Count
                });
            return await PagedList<GetPageOfSearchedStocks>.CreateAsync(selectedData, _params);
        }
        public async Task<bool> UpdateAsync(Stock stock)
        {
            _context.Entry(stock).State = EntityState.Modified;
            return (await _context.SaveChangesAsync()) > 0;
        }
        public async Task<PagedList<Get_PageOf_Stocks_ADMModel>> Get_PageOf_StockModels_ADM(StockResourceParameters _params)
        {
            var sourceData = _context.Stocks
            .OrderBy(d => d.Name)
            .Select(p => new Get_PageOf_Stocks_ADMModel
            {
                Id = p.Id,
                MgrName = p.MgrName,
                Name = p.Name,
                OwnerName = p.OwnerName,
                PersPhone = p.PersPhone,
                LandlinePhone = p.LandlinePhone,
                LicenseImgSrc = p.LicenseImgSrc,
                CommercialRegImgSrc = p.CommercialRegImgSrc,
                Status = p.Status,
                Address = $"{p.Area.SuperArea.Name}/{p.Area.Name}",
                AreaId = p.AreaId,
                joinedPharmesCount = p.GoinToPharmacies.Count,
                drugsCount=p.SDrugs.Count
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
                     .Where(p => p.Status == _params.Status);
            }
            return await PagedList<Get_PageOf_Stocks_ADMModel>.CreateAsync(sourceData, _params);
        }

        public async Task<Get_PageOf_Stocks_ADMModel> Get_StockModel_ADM(string id)
        {
            return await _context.Stocks
                .Where(p => p.Id == id)
                .Select(p => new Get_PageOf_Stocks_ADMModel
                {
                    Id = p.Id,
                    MgrName = p.MgrName,
                    Name = p.Name,
                    OwnerName = p.OwnerName,
                    PersPhone = p.PersPhone,
                    LandlinePhone = p.LandlinePhone,
                    LicenseImgSrc = p.LicenseImgSrc,
                    CommercialRegImgSrc = p.CommercialRegImgSrc,
                    Status = p.Status,
                    Address = p.Address,
                    AreaId = p.AreaId,
                    joinedPharmesCount = p.GoinToPharmacies.Count,
                })
               .SingleOrDefaultAsync();
        }
        public async Task<Stock> GetByIdAsync(string id)
        {
            return await _context.Stocks.FindAsync(id);
        }
        public async Task Delete(Stock stk)
        {
            _context.PharmaciesInStocks.RemoveRange(_context.PharmaciesInStocks.Where(ps => ps.StockId == stk.Id));
             await _context.SaveChangesAsync();
            _context.Users.Remove(_context.Users.Find(stk.Id));
        }
        public void UpdatePhone(Stock stock)
        {
            UpdateFields<Stock>(stock, prop => prop.PersPhone);
        }
        public void UpdateName(Stock stock)
        {
            UpdateFields<Stock>(stock, prop => prop.Name);
        }
        public void UpdateContacts(Stock stock)
        {
            UpdateFields<Stock>(stock,
                prop => prop.LandlinePhone,
                prop => prop.Address);
        }
        public async Task<bool> Patch_Apdate_ByAdmin(Stock stk)
        {
            return await UpdateFieldsAsync_And_Save<Stock>(stk, prop => prop.Status);
        }
        public async Task<Stock> Get_IfExists(string id)
        {
            return await _context.Stocks.FindAsync(id);
        }

        public async Task<PagedList<ShowPharmaReqToStkModel>> GetPharmaRequests(PharmaReqsResourceParameters _params)
        {
            var originalData = _context.PharmaciesInStocks
                 .Where(r => r.StockId == UserId);                
            if (!string.IsNullOrEmpty(_params.S))
            {
                var searchQueryForWhereClause = _params.S.Trim().ToLowerInvariant();
                originalData = originalData
                     .Where(d => d.Pharmacy.Name.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }
            if (_params.Status != null)
            {
                originalData = originalData
                     .Where(p => p.PharmacyReqStatus == _params.Status);
            }
            if (_params.PharmaClass != null)
            {
                originalData = originalData
                     .Where(p => p.PharmacyClass.Equals(_params.PharmaClass));
            }
            var data = originalData
                .Select(r => new ShowPharmaReqToStkModel
                {
                    PharmacyId = r.PharmacyId,
                    Seen = r.Seen,
                    Status = r.PharmacyReqStatus,
                    PharmaClass = r.PharmacyClass,

                });
            return await PagedList<ShowPharmaReqToStkModel>.CreateAsync(data, _params);
        }
        public async Task<bool> MakeRequestToStock(string stockId)
        {
            if (!_context.Stocks.Any(s => s.Id == stockId && !s.GoinToPharmacies.Any(p => p.PharmacyId == UserId)))
                return false;
            _context.PharmaciesInStocks.Add(new PharmacyInStock
            {
                PharmacyId = UserId,
                StockId = stockId
            });
            return await SaveAsync();
        }

        public async Task<bool> DeletePharmacyRequest(string PharmaId)
        {
            if (!await _context.PharmaciesInStocks.AnyAsync(s => s.StockId == UserId && s.PharmacyId == PharmaId))
                return false;
            _context.PharmaciesInStocks.Remove(
                await _context.PharmaciesInStocks
                .SingleOrDefaultAsync(r => r.PharmacyId == PharmaId && r.StockId == UserId));
            return await SaveAsync();
        }
        public async Task<bool> HandlePharmacyRequest(string pharmaId,Action<PharmacyInStock>OnRequestFounded)
        {
            var request =await _context.PharmaciesInStocks.SingleOrDefaultAsync(r=>r.StockId==UserId&&r.PharmacyId==pharmaId);
            if (request == null) return false;
            OnRequestFounded(request);
            return await UpdateFieldsAsync_And_Save<PharmacyInStock>(request, prop => prop.Seen, prop => prop.PharmacyReqStatus);
        }
        public async Task<List<string>> GetStockClassesOfJoinedPharmas(string stockId)
        {
            var classes = (await _context.Stocks
                .Where(s => s.Id == stockId)
                .Select(s => s.PharmasClasses).SingleAsync()).Split(',');

            var pharmas = _context.PharmaciesInStocks.Where(s => s.StockId == stockId);

            for(int i = 0; i < classes.Length; i++)
            {
                classes[i] = $"{classes[i]},{pharmas.Where(p=>p.PharmacyClass==classes[i]).Count()}";
            }
            return classes.ToList();
        }

        public async Task<string> AddNewPharmaClass(string newClass)
        {
            var stockModel =await _context.Stocks.FindAsync(UserId);
            if (stockModel.PharmasClasses.Split(',').Contains(newClass))
                throw new Exception("هذا التصنيف موجود بالفعل");
            stockModel.PharmasClasses += ',' + newClass;
            UpdateFields<Stock>(stockModel, s => s.PharmasClasses);
            await SaveAsync();
            return stockModel.PharmasClasses;
        }

        public async Task<string> RemovePharmaClass(string deletedClass)
        {
            var stockModel = await _context.Stocks.FindAsync(UserId);
            if (!stockModel.PharmasClasses.Split(',').Contains(deletedClass))
                throw new Exception("هذا التصنيف غير موجود");
            var classes = stockModel.PharmasClasses.Split(',');
            stockModel.PharmasClasses= classes
                .Where(c => !c.Equals(deletedClass))
                .Select(c=>c)
                 .Aggregate("", (prev, c) => "," + prev + c)
                .Remove(0, 1);
            UpdateFields<Stock>(stockModel, s => s.PharmasClasses);
            await SaveAsync();
            return stockModel.PharmasClasses;
        }

        public async Task<string> RenamePharmaClass(UpdateStockClassForPharmaModel model)
        {
            var stockModel = await _context.Stocks.FindAsync(UserId);
            var classes = stockModel.PharmasClasses.Split(',');
            if (classes.Contains(model.NewClass))
                throw new Exception("هذا التصنيف موجود بالفعل");
            if (!classes.Contains(model.OldClass))
                throw new Exception("هذا التصنيف غير موجود");
            stockModel.PharmasClasses = classes
                .Where(c => !c.Equals(model.OldClass))
                .Select(c => c)
                .Append(model.NewClass)
                .Aggregate("", (prev, c) => ","+prev + c)
                .Remove(0, 1);
            UpdateFields<Stock>(stockModel, s => s.PharmasClasses);
            await SaveAsync();
        /*    var drugs = _context.StkDrugs.Where(s => s.StockId == UserId).ToList();
            if (drugs.Count > 0)
            {
                drugs.ForEach(drug =>
                {
                    drug.Discount = drug.Discount;
                });
            }*/
            return stockModel.PharmasClasses;
        }
    }
}
