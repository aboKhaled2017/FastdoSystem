using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Repositories.Models;
using System.Threading.Tasks;
using Fastdo.backendsys.Models;
using Fastdo.backendsys.Controllers.Pharmacies;

namespace Fastdo.backendsys.Repositories
{
    public interface IPharmacyRepository:IMainRepository
    {
        Task<bool> AddAsync(Pharmacy pharmacy);
        Task<List<ShowSentRequetsToStockByPharmacyModel>> GetDentRequestsToStocks();
        IQueryable<Pharmacy> GetAllAsync();
        Task<PagedList<Get_PageOf_Pharmacies_ADMModel>> Get_PageOf_PharmacyModels_ADM(PharmaciesResourceParameters _params);
        Task<bool> UpdateAsync(Pharmacy pharmacy);
        Task<Get_PageOf_Pharmacies_ADMModel> Get_PharmacyModel_ADM(string id);
        Task<Pharmacy> GetByIdAsync(string id);
        Task Delete(Pharmacy pharm);
        void UpdatePhone(Pharmacy pharmacy);
        void UpdateName(Pharmacy pharmacy);
        void UpdateContacts(Pharmacy pharmacy);
        Task<bool> Patch_Apdate_ByAdmin(Pharmacy pharm);
        Task<Pharmacy> Get_IfExists(string id);
    }
}
