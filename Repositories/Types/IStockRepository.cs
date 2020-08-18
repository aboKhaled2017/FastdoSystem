using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Repositories.Models;
using System.Threading.Tasks;
using Fastdo.backendsys.Models;

namespace Fastdo.backendsys.Repositories
{
    public interface IStockRepository:IMainRepository
    {
         Task<bool> AddAsync(Stock stock);
         IQueryable GetAllAsync();
         Task<PagedList<Get_PageOf_Stocks_ADMModel>> Get_PageOf_StockModels_ADM(StockResourceParameters _params);
         Task<Get_PageOf_Stocks_ADMModel> Get_StockModel_ADM(string id);
         Task<bool> UpdateAsync(Stock stock);
         Task<Stock> GetByIdAsync(string id);
        void Delete(Stock stk);
        void UpdatePhone(Stock stock);
         void UpdateName(Stock stock);
         void UpdateContacts(Stock stock);
        Task<bool> Patch_Apdate_ByAdmin(Stock stk);
        Task<Stock> Get_IfExists(string id);

    }
}
