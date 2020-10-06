using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Repositories.Models;
using System.Threading.Tasks;
using Fastdo.backendsys.Models;
using Fastdo.backendsys.Controllers.Stocks.Models;
using Fastdo.backendsys.Controllers.Stocks;
using Fastdo.backendsys.Services;
using EFCore.BulkExtensions;
using Fastdo.Repositories.Enums;

namespace Fastdo.backendsys.Repositories
{
    public class StockRepository:MainRepository,IStockRepository
    {
        public StockRepository(SysDbContext context) : base(context)
        {
        }

        public async Task<bool> AddAsync(Stock stock)
        {
            
            stock.PharmasClasses=new List<StockWithPharmaClass>(){ new StockWithPharmaClass { ClassName = "الافتراضى"}};
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
            .Where(s => s.User.EmailConfirmed && !s.GoinedPharmacies.Any(g=>g.PharmacyId==UserId));
            
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
                    joinedPharmesCount = p.GoinedPharmacies.Count,
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
                Email=p.User.Email,
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
                joinedPharmesCount = p.GoinedPharmacies.Count,
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
                    joinedPharmesCount = p.GoinedPharmacies.Count,
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

        public async Task<PagedList<ShowJoinRequestToStkModel>> GetJoinRequestsPharmas(PharmaReqsResourceParameters _params)
        {
            var originalData = _context.PharmaciesInStocks
                 .Where(r => 
                 r.StockId == UserId &&
                 r.PharmacyReqStatus!=PharmacyRequestStatus.Accepted&&
                 r.PharmacyReqStatus!=PharmacyRequestStatus.Disabled);                
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
            var data = originalData
                .Select(r => new ShowJoinRequestToStkModel
                {
                    Pharma=new ShowJoinRequestToStk_pharmaDataModel
                    {
                        Id = r.PharmacyId,
                        Name = r.Pharmacy.Name,
                        AddressInDetails = r.Pharmacy.Address,
                        Address = $"{r.Pharmacy.Area.Name} / {r.Pharmacy.Area.SuperArea.Name??"غير محدد"}",
                        PhoneNumber = r.Pharmacy.PersPhone,
                        LandlinePhone = r.Pharmacy.LandlinePhone,
                    },
                    Seen = r.Seen,
                    Status = r.PharmacyReqStatus,
                    PharmaClass = r.Pharmacy.StocksClasses.SingleOrDefault(s => s.StockClass.StockId == r.StockId).StockClass.ClassName,

                });
            return await PagedList<ShowJoinRequestToStkModel>.CreateAsync(data, _params);
        }
        public async Task<PagedList<ShowJoinedPharmaToStkModel>> GetJoinedPharmas(PharmaReqsResourceParameters _params)
        {
            var originalData = _context.PharmaciesInStocks
                 .Where(r =>
                 r.StockId == UserId &&
                 (r.PharmacyReqStatus==PharmacyRequestStatus.Accepted||r.PharmacyReqStatus==PharmacyRequestStatus.Disabled));
            if (!string.IsNullOrEmpty(_params.S))
            {
                var searchQueryForWhereClause = _params.S.Trim().ToLowerInvariant();
                originalData = originalData
                     .Where(d => d.Pharmacy.Name.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }
            if (_params.PharmaClass != null)
            {
                originalData = originalData
                     .Where(p => p.Pharmacy.StocksClasses.Any(sc => sc.StockClass.StockId == p.StockId && sc.StockClass.ClassName == _params.PharmaClass));
            }
            if (_params.Status != null)
            {
                originalData = originalData
                     .Where(p => p.PharmacyReqStatus==_params.Status);
            }
            var data = originalData
                .Select(r => new ShowJoinedPharmaToStkModel
                {
                    Pharma=new ShowJoinRequestToStk_pharmaDataModel {
                        Id = r.PharmacyId,
                        Name = r.Pharmacy.Name,
                        AddressInDetails = r.Pharmacy.Address,
                        Address = $"{r.Pharmacy.Area.Name} / {r.Pharmacy.Area.SuperArea.Name ?? "غير محدد"}",
                        PhoneNumber = r.Pharmacy.PersPhone,
                        LandlinePhone = r.Pharmacy.LandlinePhone
                    },
                    PharmaClassId = r.Pharmacy.StocksClasses.SingleOrDefault(s => s.StockClass.StockId == r.StockId).StockClassId,
                    Status=r.PharmacyReqStatus

                });
            return await PagedList<ShowJoinedPharmaToStkModel>.CreateAsync(data, _params);
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

        public async Task AddNewPharmaClass(string newClass)
        {
            if(_context.StocksWithPharmaClasses.Any(s=>s.StockId==UserId && s.ClassName==newClass))
                throw new Exception("هذا التصنيف موجود بالفعل");
            _context.StocksWithPharmaClasses.Add(new StockWithPharmaClass
            {
                StockId = UserId,
                ClassName = newClass
            });
            await SaveAsync();
        }

        public async Task RemovePharmaClass(DeleteStockClassForPharmaModel model,Action<object>SendError=null)
        {

            if (!await _context.StocksWithPharmaClasses
            .AnyAsync(s=>s.Id==model.getDeletedClassId))
            {
                SendError?.Invoke(Functions.MakeError(nameof(model.DeletedClassId), "هذا التصنيف غير موجود"));
                return;
            }

            var deletedEntityClass = await _context.StocksWithPharmaClasses
                .SingleOrDefaultAsync(s =>s.Id == model.getDeletedClassId);

            var stkDrugs = new List<StkDrug>();


            //this class has subscribed pharmacies ,so they will be assigned another existed class
            if (await _context.StocksWithPharmaClasses
                .AnyAsync(s=>
                s.Id==model.getDeletedClassId
                && s.PharmaciesOfThatClass.Count > 0))
            
            {
                //get subscibed pharmacies list
                var joinedPharmasToClass = _context
                    .PharmaciesInStockClasses
                    .Where(s => s.StockClassId == deletedEntityClass.Id)
                    .ToList();
                //get the replaced existed class
                var replacedEntityClass = await _context
                    .StocksWithPharmaClasses
                    .SingleOrDefaultAsync(s => s.StockId == UserId && s.Id == model.getReplaceClassId);
                

                if (replacedEntityClass == null)
                {
                    SendError?.Invoke(Functions.MakeError(nameof(model.ReplaceClassId), "هذا التصنيف غير موجود"));
                    return;
                }

                joinedPharmasToClass.ForEach(el =>
                {
                    el.StockClassId = replacedEntityClass.Id;
                });

                await _context.BulkInsertOrUpdateOrDeleteAsync(
                    joinedPharmasToClass,
                    new BulkConfig { UpdateByProperties = new List<string> { nameof(PharmacyInStockClass.StockClassId) } }
                    );
                //get all drugs for this stock
                stkDrugs = _context.StkDrugs.Where(s => s.StockId == UserId).ToList();

                //performe edit for discount of class
                stkDrugs.ForEach(drug =>
                {
                    drug.Discount = DiscountClassifier<Guid>
                    .ReplaceClassForDiscount(drug.Discount, model.getDeletedClassId, model.getReplaceClassId);
                });

            }
            else
            {
                //get all drugs for this stock
                stkDrugs = _context.StkDrugs.Where(s => s.StockId == UserId).ToList();

                //performe edit for discount of class
                stkDrugs.ForEach(drug =>
                {
                    drug.Discount = DiscountClassifier<Guid>
                    .RemoveClassForDiscount(drug.Discount, model.getDeletedClassId);
                });                    
            }
            
            var removedStkDrugs = stkDrugs.Where(s => s.Discount == null).ToList();
            var updatedStkDrugs= stkDrugs.Where(s => s.Discount != null).ToList();
            if (removedStkDrugs.Count > 0)
            {
                _context.StkDrugs.RemoveRange(removedStkDrugs);
                await SaveAsync();
            }
            if (updatedStkDrugs.Count > 0)
                await _context.BulkUpdateAsync(updatedStkDrugs, new BulkConfig
                {
                    UpdateByProperties = new List<string> { nameof(StkDrug.Discount) }
                });
                //alwys remove this class
            _context.StocksWithPharmaClasses.Remove(deletedEntityClass);
            await SaveAsync();
        }

        public async Task RenamePharmaClass(UpdateStockClassForPharmaModel model)
        {
            var entity =await _context.StocksWithPharmaClasses.SingleOrDefaultAsync(s => s.StockId == UserId && s.ClassName == model.OldClass);
            if(entity==null)
                throw new Exception("هذا التصنيف غير موجود");
            if (_context.StocksWithPharmaClasses.Any(s => s.StockId == UserId && s.ClassName == model.NewClass))
                throw new Exception("هذا التصنيف موجود بالفعل");

            entity.ClassName = model.NewClass;
            _context.Entry(entity).State = EntityState.Modified;
            await SaveAsync();
        }
        public async Task<List<StockClassWithPharmaCountsModel>> GetStockClassesOfJoinedPharmas(string stockId)
        {
            return await _context.StocksWithPharmaClasses
                .Where(s => s.StockId == stockId)
                .Select(s => new StockClassWithPharmaCountsModel
                {
                    Id=s.Id,
                    Name = s.ClassName,
                    Count = s.PharmaciesOfThatClass.Count
                }).ToListAsync();

        }

        public List<StockClassWithPharmaCountsModel> GetStockClassesOfJoinedPharmas(Stock stock)
        {
           return stock.PharmasClasses
                .Select(s => new StockClassWithPharmaCountsModel
                {
                    Id=s.Id,
                    Name = s.ClassName,
                    Count = s.PharmaciesOfThatClass.Count
                }).ToList();
        }

        public async Task<bool> MakeRequestToStock(string stockId)
        {
            if (!_context.Stocks.Any(s => s.Id == stockId && !s.GoinedPharmacies.Any(p => p.PharmacyId == UserId)))
                return false;
            _context.PharmaciesInStocks.Add(new PharmacyInStock
            {
                PharmacyId = UserId,
                StockId = stockId
            });
            var pharmaClassId = await _context.StocksWithPharmaClasses
                .Where(s => s.StockId == stockId)
                .Select(s => s.Id).FirstOrDefaultAsync();
            _context.PharmaciesInStockClasses.Add(new PharmacyInStockClass
            {
                PharmacyId = UserId,
                StockClassId = pharmaClassId
            });
            return await SaveAsync();
        }
        public async Task<bool> CancelRequestToStock(string stockId)
        {
            var request =await _context.PharmaciesInStocks.SingleOrDefaultAsync(s => s.PharmacyId == UserId && s.StockId == stockId);
            if (request == null) return false;
            _context.PharmaciesInStockClasses.Where(p => p.PharmacyId == UserId && p.StockClass.StockId == stockId).BatchDelete();
            _context.PharmaciesInStocks.Remove(request);

            return await SaveAsync();
        }

        public async Task AssignAnotherClassForPharmacy(AssignAnotherClassForPharmacyModel model,Action<dynamic>onError)
        {
            var res =await _context.PharmaciesInStockClasses.AnyAsync(s =>
              s.PharmacyId == model.PharmaId &&
              s.StockClassId == model.getOldClassId);
            var res2 = await _context.StocksWithPharmaClasses.AnyAsync(s => s.Id == model.getNewClassId && s.StockId == UserId);
            if(!res || !res2)
            {
                onError(Functions.MakeError("انت تحاول ادخال بيانات غير صحيحة"));
                return;
            }
            var pharmaInStockClass =await _context.PharmaciesInStockClasses
                .SingleAsync(p => p.StockClassId == model.getOldClassId && p.PharmacyId == model.PharmaId);
            pharmaInStockClass.StockClassId = model.getNewClassId;
            _context.Entry(pharmaInStockClass).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
