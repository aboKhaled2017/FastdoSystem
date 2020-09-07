using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Repositories.Models;
using System.Threading.Tasks;
using Fastdo.backendsys.Models;

namespace Fastdo.backendsys.Repositories
{
    public interface IAdminRepository: IMainRepository
    {
        Task<Admin> GetByIdAsync(string id);
        Task<StatisShowModel> GetGeneralStatisOfSystem();
        Task<ShowAdminModel> GetAdminsShownModelById(string id);
        Task<bool> AddAsync(Admin admin);
        Task<PagedList<ShowAdminModel>> GET_PageOfAdminers_ShowModels_ADM(AdminersResourceParameters _params);
        void Update(Admin admin);
        Task Delete(Admin admin);
    }
}
