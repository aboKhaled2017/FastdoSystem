using Microsoft.EntityFrameworkCore;
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
        public IQueryable GetAllAsync()
        {
            return _context.Pharmacies;
        }
        public async Task<Pharmacy> GetByIdAsync(string id)
        {
            return await _context.Pharmacies.FindAsync(id);
        }
        //public async Task<>
        public async Task<bool> AddAsync(Pharmacy pharmacy)
        {
            _context.Pharmacies.Add(pharmacy);
            return (await _context.SaveChangesAsync()) > 0;
        }        
        public async Task<bool> UpdateAsync(Pharmacy pharmacy)
        {
            _context.Entry(pharmacy).State = EntityState.Modified;
            return (await _context.SaveChangesAsync()) > 0;
        }       
        public async Task<Pharmacy> DeleteAsync(string id)
        {
            var pharmacy =await  _context.Pharmacies.FindAsync(id);
            bool res=false;
            if(pharmacy!=null)
            {
                _context.Pharmacies.Remove(pharmacy);
                 res=await _context.SaveChangesAsync()>0;
            }
            return res ? pharmacy : null;
        }

    }
}
