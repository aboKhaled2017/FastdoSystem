using System;
using System.Collections.Generic;
using System.Linq;
using System.Models;
using System.Threading.Tasks;

namespace System_Back_End.Repositories
{
   public interface IAreaRepository:IMainRepository
    {
        IQueryable<Area> GetAll();
        Task<Area> GetById(byte id);
        Task<bool> Add(Area area);
        Task<Area> Delete(byte id);
        Task<bool> AreaExists(byte id);
    }
}
