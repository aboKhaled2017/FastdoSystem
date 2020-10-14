using Fastdo.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Repositories
{
    public class PharmacyInStkRepository : Repository<PharmacyInStock>, IPharmacyInStkRepository
    {
        public PharmacyInStkRepository(SysDbContext context) : base(context)
        {
        }
    }
}
