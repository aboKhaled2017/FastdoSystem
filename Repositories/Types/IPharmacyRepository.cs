using System;
using System.Collections.Generic;
using System.Linq;
using System.Models;
using System.Threading.Tasks;

namespace System_Back_End.Repositories
{
    public interface IPharmacyRepository:IMainRepository
    {
        Task<bool> AddAsync(Pharmacy pharmacy);
        IQueryable<Pharmacy> GetAllAsync();
        Task<bool> UpdateAsync(Pharmacy pharmacy);
        Task<Pharmacy> GetByIdAsync(string id);
        Task<Pharmacy> DeleteAsync(string id);
        void UpdatePhone(Pharmacy pharmacy);
        void UpdateName(Pharmacy pharmacy);
        void UpdateContacts(Pharmacy pharmacy);
    }
}
