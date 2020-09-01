using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Repositories.Models;
using System.Threading.Tasks;
using Fastdo.backendsys.Models;

namespace Fastdo.backendsys.Repositories
{
    public interface ILzDrugRepository:IMainRepository
    {
         void Add(LzDrug drug);

         Task<PagedList<Show_VStock_LzDrg_ADM_Model>> GET_PageOf_VStock_LzDrgs(LzDrgResourceParameters lzDrugResourceParameters);
         Task<PagedList<LzDrugModel_BM>> GetAll_BM(LzDrgResourceParameters lzDrugResourceParameters);

         void Update(LzDrug drug);

         Task<LzDrugModel_BM> Get_BM_ByIdAsync(Guid id);

         Task<LzDrug> GetByIdAsync(Guid id);

         void Delete(LzDrug drug);

         Task<bool> IsUserHas(Guid id);

         Task<LzDrug> GetIfExists(Guid id);

         Task<bool> LzDrugExists(Guid id);
         Task<Show_LzDrgReqDetails_ADM_Model> GEt_LzDrugDetails_For_ADM(Guid id);

    }
}
