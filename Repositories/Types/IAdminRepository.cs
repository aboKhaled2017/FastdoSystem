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
        IQueryable<ShowAdminModel> GetAllAdminsShownModels(string adminType);
        void Update(Admin admin);
        void Delete(Admin admin);
    }
}
