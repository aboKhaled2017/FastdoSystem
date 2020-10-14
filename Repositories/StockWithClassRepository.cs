using Fastdo.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Repositories
{
    public class StockWithClassRepository : Repository<StockWithPharmaClass>, IStockWithClassRepository
    {
        public StockWithClassRepository(SysDbContext context) : base(context)
        {
        }
    }
}
