using Fastdo.backendsys.Controllers.Stocks.Models;
using Fastdo.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using EFCore.BulkExtensions;
using Fastdo.backendsys.Controllers.Pharmacies;
using Newtonsoft.Json;
using System.Text;
using System.Data.Common;
using Fastdo.Core.Enums;
using NHibernate.Util;
using Microsoft.EntityFrameworkCore.Internal;
using Namotion.Reflection;

namespace Fastdo.backendsys.Repositories
{
    public class StkDrugsRepository : Repository<StkDrug>, IStkDrugsRepository
    {
        #region constructor and properties
        private IStkDrugPackgesReqsRepository _PackgesReqsRepository { get; }
        private IStkDrgInPackagesReqsRepository _StkDrgInPackagesReqs { get; }
        public StkDrugsRepository(SysDbContext context,
            IStkDrugPackgesReqsRepository packgesReqsRepository,
            IStkDrgInPackagesReqsRepository stkDrgInPackagesReqsRepository) : base(context)
        {
            _PackgesReqsRepository = packgesReqsRepository;
            _StkDrgInPackagesReqs = stkDrgInPackagesReqsRepository;
        }

        #endregion

        #region add/delete/update/get StkDrugs

        public async Task AddListOfDrugs(List<StkDrug> drugs,List<DiscountPerStkDrug> currentDrugs,string stockId)
        {
           var addedDrugs = new List<StkDrug>();
           var updatedDrugs = new List<StkDrug>();


            foreach (var drug in drugs)
            {
                if(drug.StockId==null) drug.StockId = stockId;
                if (drug.Id==Guid.Empty)
                {
                    drug.Id = Guid.NewGuid();
                    addedDrugs.Add(drug);                
                }
                else
                {
                    updatedDrugs.Add(drug);
                }
            }
           /* await _context.BulkInsertOrUpdateAsync(drugs, new BulkConfig
            {
                UpdateByProperties = new List<string> { "Price", "Discount" },
                SetOutputIdentity=true,
                PreserveInsertOrder=true
                
            });*/
            await _context.BulkInsertAsync(addedDrugs,new BulkConfig {
                SetOutputIdentity = true,
                PreserveInsertOrder = true
            });
            await _context.BulkUpdateAsync(updatedDrugs,new BulkConfig { UpdateByProperties = new List<string> {"Id"} });

            //await _context.BulkDeleteAsync(updatedDrugs, new BulkConfig { });

            
        }

        public async Task<List<DiscountPerStkDrug>> GetDiscountsForEachStockDrug(string id)
        {
            return await GetAll()
                .Where(s=>s.StockId==id)
                .Select(s => new DiscountPerStkDrug
            {
                Id=s.Id,
                Name = s.Name,
                DiscountStr = s.Discount
            }).ToListAsync();
        }

        public void DeleteAll()
        {
            GetAll().BatchDelete();
        }
        
        public async Task<PagedList<StkShowDrugModel>> GetAllStockDrugsOfReport(string id, LzDrgResourceParameters _params)
        {
            var data = GetAll()
                .Where(s => s.StockId == id)
                .Select(s => new StkShowDrugModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Discount = s.Discount,
                    Price = s.Price
                });
            if (!string.IsNullOrEmpty(_params.S))
            {
                var searchQueryForWhereClause = _params.S.Trim().ToLowerInvariant();
                data = data
                     .Where(d => d.Name.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }
            return await PagedList<StkShowDrugModel>.CreateAsync(data, _params);
        }
        public async Task<PagedList<SearchStkDrugModel_TargetPharma>> GetSearchedPageOfStockDrugsFPH(string stockId, StkDrugResourceParameters _params)
        {
            var data = GetAll()
                .Where(s => s.StockId == stockId)
               .Select(s => new SearchStkDrugModel
               {
                   Id = s.Id,
                   Name = s.Name,
                   Discount = s.Discount,
                   Price = s.Price,
                   JoinedTo=s.Stock.GoinedPharmacies.Any(p=>p.PharmacyId==UserId)
               });
            if (!string.IsNullOrEmpty(_params.S))
            {
                var searchQueryForWhereClause = _params.S.Trim().ToLowerInvariant();
                data = data
                     .Where(d => d.Name.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }
            var dataList= await PagedList<SearchStkDrugModel>.CreateAsync<SearchStkDrugModel_TargetPharma>(data, _params,drg=> {

                var entries = JsonConvert.DeserializeObject<List<Tuple<string, double>>>(drg.Discount);

                return new SearchStkDrugModel_TargetPharma
                {
                    Id = drg.Id,
                    Name = drg.Name,
                    Price = drg.Price,
                    JoinedTo=drg.JoinedTo,
                    Discount = entries.SingleOrDefault(e=>e.Item1==UserId)?.Item2
                    ??entries.Select(e=>e.Item2).Min()                   
                };
            });

            return dataList;
        }

        public async Task<PagedList<SearchGenralStkDrugModel_TargetPharma>> GetSearchedPageOfStockDrugsFPH(StkDrugResourceParameters _params)
        {
            var data = GetAll();
            if (!string.IsNullOrEmpty(_params.S))
            {
                var searchQueryForWhereClause = _params.S.Trim().ToLowerInvariant();
                data = data
                     .Where(d => d.Name.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }
            if (!string.IsNullOrEmpty(_params.StockId))
            {
                data = data.Where(s => s.StockId == _params.StockId.Trim());
            }
            var selectedData=data
                .GroupBy(s => s.Name)
               .Select(g => new SearchGenralStkDrugModel
               {
                   Name = g.Key,
                   Stocks = g.Select(d => new SearchedStkDrugModelOfAllStocks
                   {
                       Id = d.Id,
                       Discount = d.Discount,
                       Price = d.Price,
                       StockId = d.StockId,
                       StockName = d.Stock.Name,
                       IsJoinedTo = d.Stock.GoinedPharmacies.Any(p => p.PharmacyId == UserId)
                   }).Take(3),
                   StockCount = g.Count()
               });
            var dataList = await PagedList<SearchGenralStkDrugModel>
                .CreateAsync<SearchGenralStkDrugModel_TargetPharma>(selectedData, _params, drg => {
                List<Tuple<string, double>> entries = null;
                var _stocks = new List<SearchedStkDrugModelOfAllStocks_TargetPharma>();
                drg.Stocks.ToList().ForEach(d => {
                    entries = JsonConvert.DeserializeObject<List<Tuple<string, double>>>(d.Discount);
                    _stocks.Add(new SearchedStkDrugModelOfAllStocks_TargetPharma
                    {
                        Id = d.Id,
                        Price = d.Price,
                        StockId = d.StockId,
                        StockName = d.StockName,
                        IsJoinedTo=d.IsJoinedTo,
                        Discount = entries.SingleOrDefault(e => e.Item1 == UserId)?.Item2
                                  ??entries.Select(e => e.Item2).Min()                                 
                    });
                });
 
                return new SearchGenralStkDrugModel_TargetPharma
                {
                    Name = drg.Name,
                    Stocks=_stocks,
                    StockCount=drg.StockCount
                };
            });

            return dataList;
        }

        #endregion

        #region Others
        public async Task<List<StockOfStkDrugModel_TragetPharma>> GetStocksOfSpecifiedStkDrug(string stkDrgName)
        {
            var stocks = await GetAll()
                .Where(s => s.Name == stkDrgName)
                .Select(s => new StockOfStkDrugModel
                {
                    Discount = s.Discount,
                    JoinedTo = s.Stock.GoinedPharmacies.Any(p => p.PharmacyId == UserId),
                    Price = s.Price,
                    StockId = s.StockId,
                    StockName = s.Stock.Name
                }).ToListAsync();
            return stocks.Select(s => {
                var discountEntries = JsonConvert.DeserializeObject<List<Tuple<string, double>>>(s.Discount);
                return new StockOfStkDrugModel_TragetPharma
                {
                    StockName = s.StockName,
                    StockId = s.StockId,
                    JoinedTo = s.JoinedTo,
                    Price = s.Price,
                    Discount = discountEntries.FirstOrDefault(e => e.Item1 == UserId)?.Item2
                    ?? discountEntries.Select(e => e.Item2).Min()
                };
            }).ToList();
        }

        #endregion

        #region StkDrugs Packages

        //add
        public async Task MakeRequestForStkDrugsPackage(
            ShowStkDrugsPackageReqPhModel model,
            Action<dynamic> onComplete,
            Action<dynamic> onError)
        {
            await ValidatePharmaRequestFor_StkDrugsList(model.FromStocks.ToList(), _packageDetails => {

                //create new package request
                var newPackageRequest = new StkDrugPackageRequest
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name.Trim(),
                    PharmacyId = UserId,
                    PackageDetails = _packageDetails,
                    AssignedStocks = model.FromStocks.Count() > 0
                     ? GetAssignedStocksOfStkDrgPackageReq(model.FromStocks.ToList())
                     : new List<StockInStkDrgPackageReq>()
                };

                //add new package
                _PackgesReqsRepository.Add(newPackageRequest);
                SaveAsync().Wait();

                onComplete(new { newPackageRequest .Id, newPackageRequest .PackageDetails});
            }, onError);
        }

        //update 
        public async Task UpdateRequestForStkDrugsPackage(
            Guid packageId,
            ShowStkDrugsPackageReqPhModel model,
            Action<dynamic> onError)
        {
            var updatedPackageRequest = await _PackgesReqsRepository.GetAll()
                .Include(e=>e.AssignedStocks)
                .FirstOrDefaultAsync(p=>p.Id==packageId && p.PharmacyId==UserId);
            if (updatedPackageRequest == null)
            {
                onError(Functions.MakeError("هذه الباقة غير موجودة"));
                return;
            }
            await ValidatePharmaRequestFor_StkDrugsList(model.FromStocks.ToList(), _packageDetails => {
                // _context.Set<StockInStkDrgPackageReq>().RemoveRange
                updatedPackageRequest.Name = model.Name;
                updatedPackageRequest.PackageDetails = _packageDetails;
                updatedPackageRequest.AssignedStocks.Clear();
                updatedPackageRequest.AssignedStocks = model.FromStocks.Count() > 0
                ? GetAssignedStocksOfStkDrgPackageReq(model.FromStocks.ToList())
                : new List<StockInStkDrgPackageReq>();
                _context.Entry(updatedPackageRequest).State = EntityState.Modified;
                //update the package
                SaveAsync().Wait();

            }, onError,"update");
        }

        //delete
        public async Task DeleteRequestForStkDrugsPackage_FromStk(Guid packageId, Action<dynamic> onError)
        {
            var package =await _PackgesReqsRepository.GetAll()
                .Include(e=>e.AssignedStocks)
                .FirstOrDefaultAsync(e=>e.Id.Equals(packageId) && e.PharmacyId==UserId);
            if (package == null)
            {
                onError(Functions.MakeError("هذه الباقة لم تعد موجودة"));
                return;
            }
            package.AssignedStocks = new List<StockInStkDrgPackageReq>();
            _context.Entry(package).State = EntityState.Modified;
            await SaveAsync();
            _context.StkDrugPackagesRequests.Remove(package);
            await SaveAsync();
        }

        public async Task<PagedList<ShowStkDrugsPackagePhModel>> GetPageOfStkDrugsPackagesPh(StkDrugPackagePhResourceParameters _params)
        {
            var originalData = _PackgesReqsRepository.GetAll()
                .Where(e => e.PharmacyId == UserId);
            if (!string.IsNullOrEmpty(_params.S))
            {
                var searchQueryForWhereClause = _params.S.Trim().ToLowerInvariant();
                originalData = originalData
                     .Where(d => d.Name.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }            
            var data = originalData.Select(e => new ShowStkDrugsPackagePhModel(e.CreateAt)
            {
                PackageId=e.Id,
                Name = e.Name,
                 
                FromStocks = e.AssignedStocks.Select(a => new ShowStkDrugsPackagePh_FromStockModel
                {
                    Id = a.StockId,
                    Name = a.Stock.Name,
                    StockClassId=a.Stock.PharmasClasses
                    .Where(e1=>e1.PharmaciesOfThatClass.Any(e2=>e2.PharmacyId==UserId))
                    .Select(e3=>e3.Id)
                    .FirstOrDefault(),
                    Address = $"{a.Stock.Area.Name} / {a.Stock.Area.SuperArea.Name}",
                    AddressInDetails = a.Stock.Address,
                    Seen = a.Seen,
                    Status = a.Status,
                    Drugs = a.AssignedStkDrugs.Select(d => new ShowStkDrugsPackagePh_FromStock_DrugModel
                    {
                        Id = d.StkDrugId,
                        Name = d.StkDrug.Name,
                        Price = d.StkDrug.Price,
                        Quantity = d.Quantity,
                        Discount=d.StkDrug.Discount
                    })
                })
            });            
            var returnedData =await PagedList<ShowStkDrugsPackagePhModel>.CreateAsync(data, _params);
            List<Tuple<string, double>> entries = null;
            List<ShowStkDrugsPackagePh_FromStock_DrugModel> _Drugs = null;
            List<ShowStkDrugsPackagePh_FromStockModel> _FromStocks = null;

            returnedData.ForEach(d =>
            {
                _FromStocks = d.FromStocks.ToList();
                _FromStocks.ForEach(e =>
                {
                    _Drugs = e.Drugs.ToList();
                    _Drugs.ForEach(drg =>
                    {
                        entries = JsonConvert.DeserializeObject<List<Tuple<string, double>>>(drg.Discount);
                        drg.Discount = entries.FirstOrDefault(x => x.Item1 == e.Id)?.Item2 ?? entries.Select(x => x.Item2).Min();
                    });
                    e.Drugs = _Drugs;
                });
                d.FromStocks = _FromStocks;
            });
            return returnedData;
        }
        #endregion

        #region  Checkers methods

        public async Task<bool> IsUserHas(Guid id)
        {
            return await GetAll()
                .AnyAsync(d => d.Id == id && d.StockId == UserId);
        }

        public async Task<bool> LzDrugExists(Guid id)
        {
            return await GetAll()
                .AnyAsync(d => d.Id == id);
        }

        #endregion

        #region private methods

        private async Task ValidatePharmaRequestFor_StkDrugsList(            
            List<ShowStkDrugsPackageReqPh_fromStockModel> stksDrugsList,
            Action<string> OnValid,
            Action<dynamic> onError,
            string operType = "add")
        {
            if (stksDrugsList.Count == 0)
            {
                OnValid("[]");
                return;
            }
            var and_ClauseListPerStockId = new List<string>(); // list of ids
            //List of Drugs per stock = ListOf ([stockId,drugsListOfThatStock])
            stksDrugsList.ForEach(item => //one item is [stockId,drugsListOfThatStock]
            {
                string stockDrugsClause = item.DrugsList //List of [Id:Guid,Quantity:int]
                .Select(e => Guid.Parse(e.First())) //select the Id from [Id:Guid,Quantity:int]
                .Aggregate($"({nameof(StkDrug.Id)} in ('", (prev, val) => prev + val.ToString() + "','");
                and_ClauseListPerStockId
                .Add(stockDrugsClause
                .Substring(0, stockDrugsClause.Length - 2) + $") and {nameof(item.StockId)}='{item.StockId}')");
            });
            string ClauseListGroup = and_ClauseListPerStockId.Aggregate("", (prev, val) => prev + val + " or ");
            ClauseListGroup = ClauseListGroup.Substring(0, ClauseListGroup.Length - 3);
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"select count(*) from {nameof(StkDrug)}s where (");
            stringBuilder.Append($"{ClauseListGroup})");

            var expectedResult = stksDrugsList.Select(s => s.DrugsList.Count()).Aggregate(0, (prev, val) => prev + val);
            if (await ExecuteScaler<int>(stringBuilder) != expectedResult)
            {
                onError(Functions.MakeError("لقد حاولت ادخال بيانات غير صحيحة"));
                return;
            }

            //check if these drugs requested before from the same stock and not accepted
            var AllDrugsIds=stksDrugsList.ToList()
                .SelectMany(s => s.DrugsList.Select(e => Guid.Parse(e.First())));
            if (operType == "add")
            {
                var isAnyOfDrugsRequestedBefore = await _StkDrgInPackagesReqs
                    .GetAll()
                    .AnyAsync(e =>
                    e.StockPackage.Package.PharmacyId == UserId &&
                    AllDrugsIds.Contains(e.StkDrugId) &&
                    e.StockPackage.Status != StkDrugPackageRequestStatus.Completed);
                if (isAnyOfDrugsRequestedBefore)
                {
                    onError(Functions.MakeError("هذه الباقة تحتوى على راكد تم طلبه بالفعل"));
                    return;
                }
            }
            OnValid(JsonConvert.SerializeObject(stksDrugsList));
        }
        

        private List<StkDrugInStkDrgPackageReq> GetAssignedStkDrugsForStockInPackage(List<IEnumerable<dynamic>> drugsWithProps,string stockId)
        {
            var assignedDrugs = new List<StkDrugInStkDrgPackageReq>();
            drugsWithProps.ForEach(drugProps =>
            {
                assignedDrugs.Add(new StkDrugInStkDrgPackageReq
                {
                    StkDrugId=Guid.Parse(drugProps.First()),
                    Quantity=(int)drugProps.Last(),
                    StockId=stockId
                });
            });
            return assignedDrugs;
        }
       
        private List<StockInStkDrgPackageReq> GetAssignedStocksOfStkDrgPackageReq(List<ShowStkDrugsPackageReqPh_fromStockModel> stksDrugsListPerStock)
        {
            var assignedStocks = new List<StockInStkDrgPackageReq>();
            stksDrugsListPerStock.ForEach(stkDrugs => {
                assignedStocks.Add(new StockInStkDrgPackageReq
                {
                    StockId = stkDrugs.StockId,
                    AssignedStkDrugs = GetAssignedStkDrugsForStockInPackage(stkDrugs.DrugsList.ToList(), stkDrugs.StockId)
                }); ;
            });
            return assignedStocks;
        }
     
        #endregion
    }
}
