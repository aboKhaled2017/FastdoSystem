
using Fastdo.Core.Models;
using Fastdo.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.API.Repositories
{
    public class StkDrgInPackagesReqs : Repository<StkDrugInStkDrgPackageReq>, IStkDrgInPackagesReqsRepository
    {
        public StkDrgInPackagesReqs(SysDbContext context) : base(context)
        {
        }
    }
}
