using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Models;
using System.Threading.Tasks;

namespace System_Back_End.Repositories
{
    public class AreaRepository : MainRepository,IAreaRepository
    {
        public AreaRepository(SysDbContext context) : base(context)
        {
        }

        public async Task<bool> Add(Area area)
        {
            _context.Areas.Add(area);
            return await _context.SaveChangesAsync()>0;
        }

        public async Task<bool> AreaExists(byte id)
        {
            return await _context.Areas.AnyAsync(a => a.Id == id);
        }

        public async Task<Area> Delete(byte id)
        {
            var area =await _context.Areas.FindAsync(id);
            var res = false;
            if (area != null)
            {
                _context.Areas.Remove(area);
                 res=await _context.SaveChangesAsync()>0;
            }
            return res ? area : null;
        }

        public IQueryable<Area> GetAll()
        {
            return _context.Areas.AsQueryable();
        }

        public async Task<Area> GetById(byte id)
        {
            return await _context.Areas.FindAsync(id);
        }
    }
}
