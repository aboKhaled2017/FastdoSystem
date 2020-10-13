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
    public interface IStkDrugsRepository:IRepository
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
        Task<PagedList<SearchStkDrugModel_TargetPharma>> GetSearchedPageOfStockDrugsFPH(string v, StkDrugResourceParameters _params);
        Task<PagedList<SearchGenralStkDrugModel_TargetPharma>> GetSearchedPageOfStockDrugsFPH(StkDrugResourceParameters _params);
        Task<List<StockOfStkDrugModel_TragetPharma>> GetStocksOfSpecifiedStkDrug(string stkDrgName);
        Task MakeRequestForStkDrugsPackage(ShowStkDrugsPackageReqPhModel model,Action<dynamic>OnComplete, Action<dynamic>onError);
        Task DeleteRequestForStkDrugsPackage_FromStk(Guid packageId, Action<dynamic> onError);
        Task UpdateRequestForStkDrugsPackage(Guid packageId, ShowStkDrugsPackageReqPhModel model, Action<dynamic> onError);
        Task<PagedList<ShowStkDrugsPackagePhModel>> GetPageOfStkDrugsPackagesPh(StkDrugPackagePhResourceParameters _params);
    }
}
