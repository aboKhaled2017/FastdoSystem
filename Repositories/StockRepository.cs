using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Core.Models;
using System.Threading.Tasks;
using Fastdo.backendsys.Models;
using Fastdo.backendsys.Controllers.Stocks.Models;
using Fastdo.backendsys.Controllers.Stocks;
using Fastdo.backendsys.Services;
using EFCore.BulkExtensions;
using Fastdo.Core.Enums;
using Namotion.Reflection;
using Newtonsoft.Json;
using Fastdo.backendsys.Controllers.Pharmacies;

namespace Fastdo.backendsys.Repositories
{
    public class StockRepository:Repository<Stock>,IStockRepository
    {
        private IPharmacyInStkRepository _PharmacyInStkRepository { get; }
        private IStkDrgInPackagesReqsRepository _stkDrgInPackagesReqsRepository { get; }
        private IStockWithClassRepository _StockWithClassRepository { get; set; }
        private IPharmacyInStkClassRepository _PharmacyInStkClassRepository { get; set; }
        private IStkDrugsRepository _stkDrugsRepository { get; set; }
        private IStockInPackagesReqsRepository _stockInPackagesReqsRepository { get; }
        public StockRepository(SysDbContext context, 
            IPharmacyInStkRepository pharmacyInStkRepository,
            IStockWithClassRepository stockWithClassRepository,
            IPharmacyInStkClassRepository pharmacyInStkClassRepository,
            IStkDrgInPackagesReqsRepository stkDrgInPackagesReqsRepository,
            IStockInPackagesReqsRepository stockInPackagesReqsRepository,
            IStkDrugsRepository stkDrugsRepository) : base(context)
        {
            _PharmacyInStkRepository = pharmacyInStkRepository;
            _StockWithClassRepository = stockWithClassRepository;
            _PharmacyInStkClassRepository = pharmacyInStkClassRepository;
            _stkDrugsRepository = stkDrugsRepository;
            _stockInPackagesReqsRepository = stockInPackagesReqsRepository;
            _stkDrgInPackagesReqsRepository = stkDrgInPackagesReqsRepository;
        }

        public override async Task AddAsync(Stock model)
        {
            model.PharmasClasses = new List<StockWithPharmaClass>() { new StockWithPharmaClass { ClassName = "الافتراضى" } };
            await base.AddAsync(model);
            await SaveAsync();
        }
        public IQueryable GetAllAsync()
        {
            return _context.Stocks;
        }
        public async Task<PagedList<GetPageOfSearchedStocks>> GetPageOfSearchedStocks(StockSearchResourceParameters _params)
        {
            var originalData = GetAll()
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
            return await SaveAsync();
        }
        public async Task<PagedList<Get_PageOf_Stocks_ADMModel>> Get_PageOf_StockModels_ADM(StockResourceParameters _params)
        {
            var sourceData =GetAll()
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
            return await GetAll()
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

        public async Task Delete(Stock stk)
        {
            _context.PharmaciesInStocks.RemoveRange(
                _PharmacyInStkRepository
                .GetAll()
                .Where(ps => ps.StockId == stk.Id));
            await SaveAsync();
            _context.Users.Remove(_context.Users.Find(stk.Id));
        }
        public void UpdatePhone(Stock stock)
        {
            UpdateFields(stock, prop => prop.PersPhone);
        }
        public void UpdateName(Stock stock)
        {
            UpdateFields(stock, prop => prop.Name);
        }
        public void UpdateContacts(Stock stock)
        {
            UpdateFields(stock,
                prop => prop.LandlinePhone,
                prop => prop.Address);
        }
        public async Task<bool> Patch_Apdate_ByAdmin(Stock stk)
        {
            return await UpdateFieldsAsync_And_Save(stk, prop => prop.Status);
        }


        public async Task<PagedList<ShowJoinRequestToStkModel>> GetJoinRequestsPharmas(PharmaReqsResourceParameters _params)
        {
            var originalData = _PharmacyInStkRepository
                .GetAll()
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
                    PharmaClass = r.Pharmacy.StocksClasses
                    .SingleOrDefault(s => s.StockClass.StockId == r.StockId).StockClass.ClassName,

                });
            return await PagedList<ShowJoinRequestToStkModel>.CreateAsync(data, _params);
        }
        public async Task<PagedList<ShowJoinedPharmaToStkModel>> GetJoinedPharmas(PharmaReqsResourceParameters _params)
        {
            var originalData = _PharmacyInStkRepository.GetAll()
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
            var b = await originalData.ToListAsync();
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
                    PharmaClassId = r.Pharmacy.StocksClasses
                    .SingleOrDefault(s => s.StockClass.StockId == r.StockId).StockClassId,
                    Status=r.PharmacyReqStatus
                });       
            return await PagedList<ShowJoinedPharmaToStkModel>.CreateAsync(data, _params);
        }
        public async Task<bool> DeletePharmacyRequest(string PharmaId)
        {
            if (!await _PharmacyInStkRepository.GetAll()
                .AnyAsync(s => s.StockId == UserId && s.PharmacyId == PharmaId))
                return false;
           _PharmacyInStkRepository
                .Remove(
                await _PharmacyInStkRepository.GetAll()
                .SingleOrDefaultAsync(r => r.PharmacyId == PharmaId && r.StockId == UserId));
            return await SaveAsync();
        }
        public async Task<bool> HandlePharmacyRequest(string pharmaId,Action<PharmacyInStock>OnRequestFounded)
        {
            var request =await _context.PharmaciesInStocks.SingleOrDefaultAsync(r=>r.StockId==UserId&&r.PharmacyId==pharmaId);
            if (request == null) return false;
            OnRequestFounded(request);
            return await _PharmacyInStkRepository
                .UpdateFieldsAsync_And_Save(request, prop => prop.Seen, prop => prop.PharmacyReqStatus);
        }

        public async Task AddNewPharmaClass(string newClass)
        {
            if(_StockWithClassRepository.GetAll()
                .Any(s=>s.StockId==UserId && s.ClassName==newClass))
                throw new Exception("هذا التصنيف موجود بالفعل");
            _StockWithClassRepository.Add(new StockWithPharmaClass
            {
                StockId = UserId,
                ClassName = newClass
            });
            await SaveAsync();
        }

        public async Task RemovePharmaClass(DeleteStockClassForPharmaModel model,Action<object>SendError=null)
        {

            if (!await _StockWithClassRepository.GetAll()
            .AnyAsync(s=>s.Id==model.getDeletedClassId))
            {
                SendError?.Invoke(Functions.MakeError(nameof(model.DeletedClassId), "هذا التصنيف غير موجود"));
                return;
            }

            var deletedEntityClass = await _StockWithClassRepository.GetAll()
                .SingleOrDefaultAsync(s =>s.Id == model.getDeletedClassId);

            var stkDrugs = new List<StkDrug>();


            //this class has subscribed pharmacies ,so they will be assigned another existed class
            if (await _StockWithClassRepository.GetAll()
                .AnyAsync(s=>
                s.Id==model.getDeletedClassId
                && s.PharmaciesOfThatClass.Count > 0))
            
            {
                //get subscibed pharmacies list
                var joinedPharmasToClass =_PharmacyInStkClassRepository
                    .Where(s => s.StockClassId == deletedEntityClass.Id)
                    .ToList();
                //get the replaced existed class
                var replacedEntityClass = await _StockWithClassRepository.GetAll()
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
                stkDrugs = _stkDrugsRepository.Where(s => s.StockId == UserId).ToList();

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
                stkDrugs = _stkDrugsRepository
                    .Where(s => s.StockId == UserId).ToList();

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
                _stkDrugsRepository.RemoveRange(removedStkDrugs);
                await SaveAsync();
            }
            if (updatedStkDrugs.Count > 0)
                await _context.BulkUpdateAsync(updatedStkDrugs, new BulkConfig
                {
                    UpdateByProperties = new List<string> { nameof(StkDrug.Discount) }
                });
                //alwys remove this class
            _StockWithClassRepository.Remove(deletedEntityClass);
            await SaveAsync();
        }

        public async Task RenamePharmaClass(UpdateStockClassForPharmaModel model)
        {
            var stocksWithPhClasses = _StockWithClassRepository.GetAll();
            var entity =await stocksWithPhClasses
                 .SingleOrDefaultAsync(s => s.StockId == UserId && s.ClassName == model.OldClass);
            if(entity==null)
                throw new Exception("هذا التصنيف غير موجود");
            if (stocksWithPhClasses
                .Any(s => s.StockId == UserId && s.ClassName == model.NewClass))
                throw new Exception("هذا التصنيف موجود بالفعل");

            entity.ClassName = model.NewClass;
            _context.Entry(entity).State = EntityState.Modified;
            await SaveAsync();
        }
        public async Task<List<StockClassWithPharmaCountsModel>> GetStockClassesOfJoinedPharmas(string stockId)
        {
            return await _StockWithClassRepository
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
            if (!GetAll().Any(s => s.Id == stockId && !s.GoinedPharmacies.Any(p => p.PharmacyId == UserId)))
                return false;
            _PharmacyInStkRepository.Add(new PharmacyInStock
            {
                PharmacyId = UserId,
                StockId = stockId
            });
            var pharmaClassId = await _StockWithClassRepository
                .Where(s => s.StockId == stockId)
                .Select(s => s.Id).FirstOrDefaultAsync();
            _PharmacyInStkClassRepository.Add(new PharmacyInStockClass
            {
                PharmacyId = UserId,
                StockClassId = pharmaClassId
            });
            return await SaveAsync();
        }
        public async Task<bool> CancelRequestToStock(string stockId)
        {
            var request =await _PharmacyInStkRepository
                .GetAll()
                .SingleOrDefaultAsync(s => s.PharmacyId == UserId && s.StockId == stockId);
            if (request == null) return false;
            _PharmacyInStkClassRepository
                .Where(p => p.PharmacyId == UserId && p.StockClass.StockId == stockId).BatchDelete();
            _PharmacyInStkRepository.Remove(request);

            return await SaveAsync();
        }

        public async Task AssignAnotherClassForPharmacy(AssignAnotherClassForPharmacyModel model,Action<dynamic>onError)
        {
            var pharmaInStkClasses = _PharmacyInStkClassRepository.GetAll();
            var res =await pharmaInStkClasses
                .AnyAsync(s =>
              s.PharmacyId == model.PharmaId &&
              s.StockClassId == model.getOldClassId);
            var res2 = await _StockWithClassRepository
                .GetAll()
                .AnyAsync(s => s.Id == model.getNewClassId && s.StockId == UserId);
            if(!res || !res2)
            {
                onError(Functions.MakeError("انت تحاول ادخال بيانات غير صحيحة"));
                return;
            }
            var pharmaInStockClass =await pharmaInStkClasses
                .SingleAsync(p => p.StockClassId == model.getOldClassId && p.PharmacyId == model.PharmaId);
            pharmaInStockClass.StockClassId = model.getNewClassId;
            _context.Entry(pharmaInStockClass).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task HandleStkDrugsPackageRequest_ForStock(Guid packageId, Action<dynamic> onProcess, Action<dynamic> onError)
        {
            var request =await _stockInPackagesReqsRepository.GetAll()
                .FirstOrDefaultAsync(e=>e.PackageId.Equals(packageId) && e.StockId==UserId);
            if (request == null)
            {
                onError(Functions.MakeError("هذا الطلب غير موجود"));
                return;
            }
            onProcess(request);
            _context.Entry(request).State = EntityState.Modified;
            await SaveAsync();
        }

        public async Task<PagedList<StkDrugsPackageReqModel>> GetStkDrugsPackageRequests(StkDrugsPackageReqResourceParmaters _params)
        {
            var originalData = _stockInPackagesReqsRepository
                 .Where(r =>
                 r.StockId==UserId &&
                 r.Status!=StkDrugPackageRequestStatus.Completed);

            if (_params.Status != null)
            {
                originalData = originalData
                     .Where(p => p.Status == _params.Status);
            }
            var data = originalData
                .Select(r => new StkDrugsPackageReqModel
                {
                    StkPackageId = r.Id,
                    PackageId = r.PackageId,
                    Status = r.Status,
                    CreatedAt = r.Package.CreateAt,
                    Pharma = new StkDrugsPackageReqModel_PharmaData
                    {
                        Id = r.Package.PharmacyId,
                        Name = r.Package.Pharmacy.Name,
                        Address = $"{r.Package.Pharmacy.Area.Name} / {r.Package.Pharmacy.Area.SuperArea.Name}",
                        AddressInDetails = r.Package.Pharmacy.Address,
                        LandLinePhone = r.Package.Pharmacy.LandlinePhone,
                        PhoneNumber = r.Package.Pharmacy.PersPhone
                    },
                    Drugs = r.AssignedStkDrugs.Select(d => new StkDrugsPackageReqModel_DrugData
                    {
                        Id = d.StkDrugId,
                        Name = d.StkDrug.Name,
                        Quantity = d.Quantity
                    })
                });
            return await PagedList<StkDrugsPackageReqModel>.CreateAsync(data, _params);
        }

        public async Task<IList<StockNameWithIdModel>> GetAllStocksNames()
        {
            return await GetAll()
                .Select(s => new StockNameWithIdModel
            {
                Id = s.Id,
                Name = s.Name
            }).ToListAsync();
        }
    }
}
