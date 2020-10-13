using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Repositories.Models;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Repositories
{
   public interface IAreaRepository:IRepository<Area>
    {
        Task<bool> AreaExists(byte id);
    }
}
