using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Models;
using System.Threading.Tasks;
using System_Back_End.Models;

namespace System_Back_End.Repositories
{
    public interface ILzDrugRepository:IMainRepository
    {
         void Add(LzDrug drug);

         Task<PagedList<ShowLzDrugModel>> GetAll_BM(LzDrugResourceParameters lzDrugResourceParameters);

         void Update(LzDrug drug);

         Task<ShowLzDrugModel> Get_BM_ByIdAsync(Guid id);

         Task<LzDrug> GetByIdAsync(Guid id);

         void Delete(LzDrug drug);

         Task<bool> IsUserHas(Guid id);

         Task<LzDrug> GetIfExists(Guid id);

         Task<bool> LzDrugExists(Guid id);

    }
}
