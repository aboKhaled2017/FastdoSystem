using Fastdo.backendsys.Controllers.Stocks.Models;
using Fastdo.Repositories.Models;
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
using Fastdo.Repositories.Enums;

namespace Fastdo.backendsys.Repositories
{
    public class StkDrugsRepository : MainRepository, IStkDrugsRepository
    {
        #region constructor and properties
        public StkDrugsRepository(SysDbContext context) : base(context)
        {
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
            return await _context.StkDrugs
                .Where(s=>s.StockId==id)
                .Select(s => new DiscountPerStkDrug
            {
                Id=s.Id,
                Name = s.Name,
                DiscountStr = s.Discount
            }).ToListAsync();
        }

        public async Task<StkDrug> GetByIdAsync(Guid id)
        {
            return await _context.StkDrugs.FindAsync(id);
        }
        public void Delete(StkDrug drug)
        {
            _context.StkDrugs.Remove(drug);
        }
        public void DeleteAll()
        {
            _context.StkDrugs.BatchDelete();
        }
        
        public async Task<PagedList<StkShowDrugModel>> GetAllStockDrugsOfReport(string id, LzDrgResourceParameters _params)
        {
            var data = _context.StkDrugs.Where(s => s.StockId == id)
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
        public async Task<PagedList<SearchStkDrugModel_TargetPharma>> GetSearchedPageOfStockDrugsFPH(string stockId, LzDrgResourceParameters _params)
        {
            var data = _context.StkDrugs.Where(s => s.StockId == stockId)
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

        public async Task<PagedList<SearchGenralStkDrugModel_TargetPharma>> GetSearchedPageOfStockDrugsFPH(LzDrgResourceParameters _params)
        {
            var data = _context.StkDrugs
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
                       IsJoinedTo=d.Stock.GoinedPharmacies.Any(p=>p.PharmacyId==UserId)
                   }).Take(3),
                   StockCount=g.Count()
               });
            if (!string.IsNullOrEmpty(_params.S))
            {
                var searchQueryForWhereClause = _params.S.Trim().ToLowerInvariant();
                data = data
                     .Where(d => d.Name.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }
            var dataList = await PagedList<SearchGenralStkDrugModel>.CreateAsync<SearchGenralStkDrugModel_TargetPharma>(data, _params, drg => {
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
            var stocks = await _context.StkDrugs
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
            IEnumerable<StkDrugsReqOfPharmaModel> stksDrugsList,
            Action<dynamic> onComplete,
            Action<dynamic> onError)
        {
            await ValidatePharmaRequestFor_StkDrugsList(stksDrugsList, _packageDetails => {

                //create new package request
                var newPackageRequest = new StkDrugPackageRequest
                {
                    Id = Guid.NewGuid(),
                    PharmacyId = UserId,
                    PackageDetails = _packageDetails,
                    AssignedStocks=GetAssignedStocksOfStkDrgPackageReq(stksDrugsList.ToList())
                };

                //add new package
                _context.StkDrugPackagesRequests.Add(newPackageRequest);
                SaveAsync().Wait();

                onComplete(new { newPackageRequest .Id, newPackageRequest .PackageDetails});
            }, onError);
        }

        //update 
        public async Task UpdateRequestForStkDrugsPackage(
            Guid packageId,
            IEnumerable<StkDrugsReqOfPharmaModel> stksDrugsList,
            Action<dynamic> onError)
        {
            var updatedPackageRequest = await _context.StkDrugPackagesRequests
                .Include(e=>e.AssignedStocks)
                .FirstOrDefaultAsync(p=>p.Id==packageId && p.PharmacyId==UserId);
            if (updatedPackageRequest == null)
            {
                onError(Functions.MakeError("هذه الباقة غير موجودة"));
                return;
            }
            await ValidatePharmaRequestFor_StkDrugsList(stksDrugsList, _packageDetails => {
               // _context.Set<StockInStkDrgPackageReq>().RemoveRange
                updatedPackageRequest.PackageDetails = _packageDetails;
                updatedPackageRequest.AssignedStocks.Clear();
                updatedPackageRequest.AssignedStocks=GetAssignedStocksOfStkDrgPackageReq(stksDrugsList.ToList());
                _context.Entry(updatedPackageRequest).State = EntityState.Modified;
                //update the package
                SaveAsync().Wait();

            }, onError,"update");
        }

        //delete
        public async Task DeleteRequestForStkDrugsPackage_FromStk(Guid packageId, Action<dynamic> onError)
        {
            var package =await _context.StkDrugPackagesRequests
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

        #endregion

        #region  Checkers methods

        public async Task<bool> IsUserHas(Guid id)
        {
            return await _context.StkDrugs.AnyAsync(d => d.Id == id && d.StockId == UserId);
        }
        public async Task<StkDrug> GetIfExists(Guid id)
        {
            return await _context.StkDrugs.FindAsync(id);
        }
        public async Task<bool> LzDrugExists(Guid id)
        {
            return await _context.StkDrugs.AnyAsync(d => d.Id == id);
        }

        #endregion

        #region private methods

        private async Task ValidatePharmaRequestFor_StkDrugsList(            
            IEnumerable<StkDrugsReqOfPharmaModel> stksDrugsList,
            Action<string> OnValid,
            Action<dynamic> onError,
            string operType = "add")
        {
            var and_ClauseListPerStockId = new List<string>(); // list of ids
            //List of Drugs per stock = ListOf ([stockId,drugsListOfThatStock])
            stksDrugsList.ToList().ForEach(item => //one item is [stockId,drugsListOfThatStock]
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
                var isAnyOfDrugsRequestedBefore = await _context.StkDrugInStkDrgPackagesRequests
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
                    StockId=stockId
                });
            });
            return assignedDrugs;
        }
       
        private List<StockInStkDrgPackageReq> GetAssignedStocksOfStkDrgPackageReq(List<StkDrugsReqOfPharmaModel> stksDrugsListPerStock)
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
       
        private async Task UpdateRequestedPackedDrugsToDbWithReferenceToItsPackage(
            Guid packageId,
            IEnumerable<StkDrugsReqOfPharmaModel> stksDrugsList,
            Action<dynamic> onError)
        {
            var oldPackageEntries = await _context
                .StkDrugInStkDrgPackagesRequests
                .Where(s => s.StockPackage.PackageId == packageId)
                .ToListAsync();
            var newpackageEntries = new List<StkDrugInStkDrgPackageReq>();
            var removedpackageEntries = new List<StkDrugInStkDrgPackageReq>();

            var recievedDrugsIds = stksDrugsList.ToList()
                .SelectMany(e => e.DrugsList.Select(e1 => (Guid)Guid.Parse(e1.First())))
                .ToList();

            recievedDrugsIds.ForEach(drugId =>
            {
                var oldDrug = oldPackageEntries.FirstOrDefault(d => d.StkDrugId == drugId);
                if (oldDrug!=null)
                {
                    newpackageEntries.Add(oldDrug);
                }
                else
                {
                    newpackageEntries.Add(new StkDrugInStkDrgPackageReq
                    {
                        Id = Guid.NewGuid(),
                        StkDrugId = drugId,
                        //StkDrugPackageId = packageId
                    });
                }
            });

            oldPackageEntries.ForEach(entry =>
            {
                if (!recievedDrugsIds.Contains(entry.StkDrugId))
                    removedpackageEntries.Add(entry);
            });
            await _context.BulkDeleteAsync(removedpackageEntries);
            
            await _context.BulkInsertOrUpdateAsync(newpackageEntries, new BulkConfig { SetOutputIdentity = true, PreserveInsertOrder = true });
            
        }

        #endregion
    }
}
