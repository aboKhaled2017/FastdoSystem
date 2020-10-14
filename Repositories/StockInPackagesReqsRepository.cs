using Fastdo.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Repositories
{
    public class StockInPackagesReqsRepository : Repository<StockInStkDrgPackageReq>, IStockInPackagesReqsRepository
    {
        public StockInPackagesReqsRepository(SysDbContext context) : base(context)
        {
        }
    }
}
