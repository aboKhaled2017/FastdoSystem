using Fastdo.backendsys.Controllers.Stocks.Models;
using Fastdo.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Repositories
{
    public interface IStkDrugsRepository:IMainRepository
    {
        Task AddListOfDrugs(List<StkDrug> drugs, List<DiscountPerStkDrug> currentDrugs,string stkId);
        Task<List<DiscountPerStkDrug>> GetDiscountsForEachStockDrug(string id);
        Task<PagedList<StkShowDrugModel>> GetAllStockDrugsOfReport(string id, LzDrgResourceParameters _params);
        Task<StkDrug> GetByIdAsync(Guid id);
        void Delete (StkDrug drug);
        void DeleteAll();
        Task<bool> IsUserHas(Guid id);

        Task<StkDrug> GetIfExists(Guid id);

        Task<bool> LzDrugExists(Guid id);

    }
}
