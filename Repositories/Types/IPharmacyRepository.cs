using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Repositories.Models;
using System.Threading.Tasks;
using Fastdo.backendsys.Models;

namespace Fastdo.backendsys.Repositories
{
    public interface IPharmacyRepository:IMainRepository
    {
        Task<bool> AddAsync(Pharmacy pharmacy);
        IQueryable<Pharmacy> GetAllAsync();
        Task<PagedList<Get_PageOf_Pharmacies_ADMModel>> Get_PageOf_PharmacyModels_ADM(PharmaciesResourceParameters _params);
        Task<bool> UpdateAsync(Pharmacy pharmacy);
        Task<Get_PageOf_Pharmacies_ADMModel> Get_PharmacyModel_ADM(string id);
        Task<Pharmacy> GetByIdAsync(string id);
        void Delete(Pharmacy pharm);
        void UpdatePhone(Pharmacy pharmacy);
        void UpdateName(Pharmacy pharmacy);
        void UpdateContacts(Pharmacy pharmacy);
        Task<bool> Patch_Apdate_ByAdmin(Pharmacy pharm);
        Task<Pharmacy> Get_IfExists(string id);
    }
}
