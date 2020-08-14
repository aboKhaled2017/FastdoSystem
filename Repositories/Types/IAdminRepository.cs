using System;
using System.Collections.Generic;
using System.Linq;
using System.Models;
using System.Threading.Tasks;
using System_Back_End.Models;

namespace System_Back_End.Repositories
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
