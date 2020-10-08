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

namespace Fastdo.backendsys.Repositories
{
    public class StkDrugsRepository : MainRepository, IStkDrugsRepository
    {
        #region constructor and properties
        public StkDrugsRepository(SysDbContext context) : base(context)
        {
        }

        #endregion

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

        public async Task MakePharmaReqToListOfStkDrugs(IEnumerable<StkDrugsReqOfPharmaModel> stkDrugsList, Action<dynamic> onError)
        {
           
            var idsList = new List<string>();
            stkDrugsList.ToList().ForEach(item =>
            {
                string idsStr = item.DrugsList
                .Select(e=>Guid.Parse(e.First()))
                .Aggregate($"({nameof(StkDrug.Id)} in ('", (prev, val) => prev + val.ToString() + "','");
                idsList.Add(idsStr.Substring(0,idsStr.Length-2)+ $") and {nameof(item.StockId)}='{item.StockId}')");
            });
            string AllIdsStr = idsList.Aggregate("", (prev, val) => prev + val+" or ");
            AllIdsStr = AllIdsStr.Substring(0,AllIdsStr.Length - 3);
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"select count(*) from {nameof(StkDrug)}s where (");
            stringBuilder.Append($"{AllIdsStr})");
            
            var expectedResult = stkDrugsList.Select(s => s.DrugsList.Count()).Aggregate(0, (prev, val) => prev + val);
            if(await ExecuteScaler<int>(stringBuilder) != expectedResult){
                onError(Functions.MakeError("لقد حاولت ادخال بيانات غير صحيحة"));
                return;
            }
            
            var newPackageRequest = new StkDrugPackageRequest
            {
                Id=Guid.NewGuid(),
                PharmacyId = UserId,
                PackageDetails = JsonConvert.SerializeObject(stkDrugsList)
            };
            _context.StkDrugPackagesRequests.Add(newPackageRequest);

            await SaveAsync();

            var packageEntries = new List<StkDrugInStkDrgPackageReq>();

            stkDrugsList.ToList().ForEach(drg =>
            {
                drg.DrugsList.ToList().ForEach(e =>
                {
                    packageEntries.Add(new StkDrugInStkDrgPackageReq
                    {
                        Id=Guid.NewGuid(),
                        StkDrugId = Guid.Parse(e.First()),
                        StkDrugPackageId = newPackageRequest.Id
                    });
                });
            });

            var res= _context.BulkInsertOrUpdateAsync(packageEntries, new BulkConfig { SetOutputIdentity = true ,PreserveInsertOrder=true});
            await res;
            if(!res.IsCompletedSuccessfully)
                onError(Functions.MakeError("حدثت مشكلى اثناء معالجة الطلب"));
            
        }
    }
}
