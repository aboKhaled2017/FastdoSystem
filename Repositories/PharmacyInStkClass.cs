using Fastdo.Core.Models;
using Fastdo.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.API.Repositories
{
    public class PharmacyInStkClassRepository : Repository<PharmacyInStockClass>, IPharmacyInStkClassRepository
    {
        public PharmacyInStkClassRepository(SysDbContext context) : base(context)
        {
        }
    }
}
