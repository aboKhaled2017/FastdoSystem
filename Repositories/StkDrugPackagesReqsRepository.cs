using Fastdo.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Repositories
{
    public class StkDrugPackagesReqsRepository : Repository<StkDrugPackageRequest>, IStkDrugPackgesReqsRepository
    {
        public StkDrugPackagesReqsRepository(SysDbContext context) : base(context)
        {
        }
    }
}
