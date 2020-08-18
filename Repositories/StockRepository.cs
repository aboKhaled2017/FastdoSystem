using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Repositories.Models;
using System.Threading.Tasks;
using Fastdo.backendsys.Models;

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
                Address = p.Address,
                AreaId = p.AreaId,
                joinedPharmesCount = p.GoinToPharmacies.Count,
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
        public void Delete(Stock stk)
        {
            _context.Stocks.Remove(stk);
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
    }
}
