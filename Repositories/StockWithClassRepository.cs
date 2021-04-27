using Fastdo.Core.Models;
using Fastdo.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.API.Repositories
{
    public class StockWithClassRepository : Repository<StockWithPharmaClass>, IStockWithClassRepository
    {
        public StockWithClassRepository(SysDbContext context) : base(context)
        {
        }
    }
}
