using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fastdo.backendsys.Models;
using Fastdo.Core.Models;
using Fastdo.Core;

namespace Fastdo.backendsys.Repositories
{
    public interface ILzDrg_Search_Repository:IRepository<LzDrug>
    {
        Task<PagedList<LzDrugCard_Info_BM>> Get_All_LzDrug_Cards_BMs(LzDrg_Card_Info_BM_ResourceParameters _params);
    }
}
