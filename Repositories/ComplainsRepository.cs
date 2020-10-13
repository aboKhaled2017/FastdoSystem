using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Repositories.Models;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Repositories
{
    public class ComplainsRepository : Repository, IComplainsRepository
    {
        public ComplainsRepository(SysDbContext context) : base(context)
        {
        }

        public async Task<bool> Add(Complain complain)
        {
            _context.Complains.Add(complain);
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<bool> ComplainExists(Guid id)
        {
            return await _context.Complains.AnyAsync(e => e.Id == id);
        }

        public async Task<Complain> Delete(Guid id)
        {
            var complain = await _context.Complains.FindAsync(id);
            if (complain!=null)
            {
                _context.Complains.Remove(complain);
                await _context.SaveChangesAsync();
            }
            return complain;
        }

        public IQueryable<Complain> GetAll()
        {
            return _context.Complains.AsQueryable();
        }

        public async Task<Complain> GetById(Guid id)
        {
            return await _context.Complains.FindAsync(id);
        }

        public async Task<bool> Update(Complain complain)
        {
            _context.Entry(complain).State = EntityState.Modified;
            return (await _context.SaveChangesAsync()) >0;
        }
    }
}
