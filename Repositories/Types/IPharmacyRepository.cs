using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Core.Models;
using System.Threading.Tasks;
using Fastdo.backendsys.Models;
using Fastdo.backendsys.Controllers.Pharmacies;
using Fastdo.Core;

namespace Fastdo.backendsys.Repositories
{
    public interface IPharmacyRepository:IRepository<Pharmacy>
    {
        Task<List<List<string>>> GetPharmaClassesOfJoinedStocks(string pharmaId);
        Task<IEnumerable<ShowJoinedStocksOfPharmaModel>> GetUserJoinedStocks();
        Task<List<ShowSentRequetsToStockByPharmacyModel>> GetSentRequestsToStocks();

        Task<PagedList<Get_PageOf_Pharmacies_ADMModel>> Get_PageOf_PharmacyModels_ADM(PharmaciesResourceParameters _params);
        Task<bool> UpdateAsync(Pharmacy pharmacy);
        Task<Get_PageOf_Pharmacies_ADMModel> Get_PharmacyModel_ADM(string id);
        void UpdatePhone(Pharmacy pharmacy);
        void UpdateName(Pharmacy pharmacy);
        void UpdateContacts(Pharmacy pharmacy);
        Task<bool> Patch_Apdate_ByAdmin(Pharmacy pharm);

    }
}
