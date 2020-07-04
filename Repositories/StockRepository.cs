using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Models;
using System.Threading.Tasks;

namespace System_Back_End.Repositories
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
        public async Task<Stock> GetByIdAsync(string id)
        {
            return await _context.Stocks.FindAsync(id);
        }
        public async Task<Stock> DeleteAsync(string id)
        {
            var stock = await _context.Stocks.FindAsync(id);
            bool res = false;
            if (stock != null)
            {
                _context.Stocks.Remove(stock);
                res = await _context.SaveChangesAsync() > 0;
            }
            return res ? stock : null;
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
                prop => prop.PersPhone,
                prop => prop.LandlinePhone,
                prop => prop.Address);
        }
    }
}
