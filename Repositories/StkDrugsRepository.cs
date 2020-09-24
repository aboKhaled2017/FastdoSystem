using Fastdo.backendsys.Controllers.Stocks.Models;
using Fastdo.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using EFCore.BulkExtensions;

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
    }
}
