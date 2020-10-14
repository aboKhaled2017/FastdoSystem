
using Fastdo.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Repositories
{
    public class StkDrgInPackagesReqs : Repository<StkDrugInStkDrgPackageReq>, IStkDrgInPackagesReqsRepository
    {
        public StkDrgInPackagesReqs(SysDbContext context) : base(context)
        {
        }
    }
}
