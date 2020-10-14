using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Core.Models;
using System.Threading.Tasks;
using Fastdo.backendsys.Models;
using Fastdo.backendsys.Controllers.Stocks.Models;
using Fastdo.backendsys.Controllers.Stocks;
using Fastdo.backendsys.Controllers.Pharmacies;
using Fastdo.Core;

namespace Fastdo.backendsys.Repositories
{
    public interface IStockRepository:IRepository<Stock>
    {
        Task<List<StockClassWithPharmaCountsModel>> GetStockClassesOfJoinedPharmas(string stockId);
        List<StockClassWithPharmaCountsModel> GetStockClassesOfJoinedPharmas(Stock stock);
        Task<PagedList<ShowJoinRequestToStkModel>> GetJoinRequestsPharmas(PharmaReqsResourceParameters _params);
        Task<PagedList<ShowJoinedPharmaToStkModel>> GetJoinedPharmas(PharmaReqsResourceParameters _params);
        Task<bool> HandlePharmacyRequest(string pharmaId, Action<PharmacyInStock> OnRequestFounded);
        Task<bool> DeletePharmacyRequest(string PharmaId);
        Task<bool> MakeRequestToStock(string stockId);
        Task<PagedList<GetPageOfSearchedStocks>> GetPageOfSearchedStocks(StockSearchResourceParameters _params);
       
         Task<PagedList<Get_PageOf_Stocks_ADMModel>> Get_PageOf_StockModels_ADM(StockResourceParameters _params);
         Task<Get_PageOf_Stocks_ADMModel> Get_StockModel_ADM(string id);
         Task<bool> UpdateAsync(Stock stock);

         void UpdatePhone(Stock stock);
         void UpdateName(Stock stock);
         void UpdateContacts(Stock stock);
        Task<bool> Patch_Apdate_ByAdmin(Stock stk);
     
        Task AddNewPharmaClass(string newClass);
        Task RemovePharmaClass(DeleteStockClassForPharmaModel model,Action<object>SendError= null);
        Task RenamePharmaClass(UpdateStockClassForPharmaModel model);
        Task<bool> CancelRequestToStock(string stockId);
        Task AssignAnotherClassForPharmacy(AssignAnotherClassForPharmacyModel model, Action<dynamic>onError);
        Task HandleStkDrugsPackageRequest_ForStock(Guid packageId, Action<dynamic> onProcess, Action<dynamic> onError);
        Task<PagedList<StkDrugsPackageReqModel>> GetStkDrugsPackageRequests(StkDrugsPackageReqResourceParmaters _params);
        Task<IList<StockNameWithIdModel>> GetAllStocksNames();
    }
}
