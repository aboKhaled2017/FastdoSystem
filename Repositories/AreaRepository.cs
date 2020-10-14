using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Core.Models;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Repositories
{
    public class AreaRepository : Repository<Area>,IAreaRepository
    {
        public AreaRepository(SysDbContext context) : base(context)
        {
        }

        public async Task<bool> AreaExists(byte id)
        {           
            return await GetAll().AnyAsync(a => a.Id == id);
        }

        public async Task<Area> Delete(byte id)
        {
            var area =await GetByIdAsync(id);
            var res = false;
            if (area != null)
            {
                Remove(area);
                res = await SaveAsync();
            }
            return res ? area : null;
        }

    }
}
