using Fastdo.Core.Models;
using Fastdo.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.API.Repositories
{
    public class PharmacyInStkRepository : Repository<PharmacyInStock>, IPharmacyInStkRepository
    {
        public PharmacyInStkRepository(SysDbContext context) : base(context)
        {
        }
    }
}
