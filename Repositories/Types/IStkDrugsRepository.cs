using Fastdo.backendsys.Controllers.Pharmacies;
using Fastdo.backendsys.Controllers.Stocks.Models;
using Fastdo.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
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
        Task<PagedList<SearchStkDrugModel_TargetPharma>> GetSearchedPageOfStockDrugsFPH(string v, LzDrgResourceParameters _params);
        Task<PagedList<SearchGenralStkDrugModel_TargetPharma>> GetSearchedPageOfStockDrugsFPH(LzDrgResourceParameters _params);
        Task<List<StockOfStkDrugModel_TragetPharma>> GetStocksOfSpecifiedStkDrug(string stkDrgName);
        Task MakePharmaReqToListOfStkDrugs(IEnumerable<StkDrugsReqOfPharmaModel> stkDrugsList, Action<dynamic>onError);
    }
}
