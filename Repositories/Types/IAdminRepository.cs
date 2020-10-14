using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Core.Models;
using System.Threading.Tasks;
using Fastdo.backendsys.Models;
using Fastdo.Core;

namespace Fastdo.backendsys.Repositories
{
    public interface IAdminRepository: IRepository<Admin>
    {
        Task<StatisShowModel> GetGeneralStatisOfSystem();
        Task<ShowAdminModel> GetAdminsShownModelById(string id);

        Task<PagedList<ShowAdminModel>> GET_PageOfAdminers_ShowModels_ADM(AdminersResourceParameters _params);
        void Update(Admin admin);
    }
}
