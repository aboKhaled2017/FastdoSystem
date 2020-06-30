using System;
using System.Collections.Generic;
using System.Linq;
using System.Models;
using System.Threading.Tasks;

namespace System_Back_End.Repositories
{
    public class StockRepository:MainRepository
    {
        public StockRepository(SysDbContext context) : base(context)
        {
        }

        public bool Add(Stock stock)
        {
            _context.Stocks.Add(stock);
            return _context.SaveChanges() > 0;
        }
    }
}
