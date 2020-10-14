using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Core.Models;
using System.Threading.Tasks;
using Fastdo.Core;

namespace Fastdo.backendsys.Repositories
{
   public interface IAreaRepository:IRepository<Area>
    {
        Task<bool> AreaExists(byte id);
    }
}
