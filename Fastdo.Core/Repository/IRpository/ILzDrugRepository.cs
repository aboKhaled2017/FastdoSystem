using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Core.Models;
using System.Threading.Tasks;
using Fastdo.Core.ViewModels;
using Fastdo.Core;

namespace Fastdo.Core.Repositories
{
    public interface ILzDrugRepository:IRepository<LzDrug>
    {
         Task<PagedList<Show_VStock_LzDrg_ADM_Model>> GET_PageOf_VStock_LzDrgs(LzDrgResourceParameters lzDrugResourceParameters);
         Task<PagedList<LzDrugModel_BM>> GetAll_BM(LzDrgResourceParameters lzDrugResourceParameters);

         void Update(LzDrug drug);

         Task<LzDrugModel_BM> Get_BM_ByIdAsync(Guid id);



         Task<bool> IsUserHas(Guid id);


         Task<bool> LzDrugExists(Guid id);
         Task<Show_LzDrgReqDetails_ADM_Model> GEt_LzDrugDetails_For_ADM(Guid id);

    }
}
