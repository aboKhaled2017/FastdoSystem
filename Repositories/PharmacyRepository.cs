using System;
using System.Collections.Generic;
using System.Linq;
using System.Models;
using System.Threading.Tasks;

namespace System_Back_End.Repositories
{
    public class PharmacyRepository:MainRepository
    {
        public PharmacyRepository(SysDbContext context) : base(context)
        {
        }

        public bool Add(Pharmacy pharmacy)
        {
            _context.Pharmacies.Add(pharmacy);
            return _context.SaveChanges() > 0;
        }
    }
}
