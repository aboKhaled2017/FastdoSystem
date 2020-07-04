using System;
using System.Collections.Generic;
using System.Linq;
using System.Models;
using System.Threading.Tasks;

namespace System_Back_End.Repositories
{
    public interface IStockRepository:IMainRepository
    {
         Task<bool> AddAsync(Stock stock);
         IQueryable GetAllAsync();
         Task<bool> UpdateAsync(Stock stock);
         Task<Stock> GetByIdAsync(string id);
         Task<Stock> DeleteAsync(string id);
         void UpdatePhone(Stock stock);
         void UpdateName(Stock stock);
         void UpdateContacts(Stock stock);

    }
}
