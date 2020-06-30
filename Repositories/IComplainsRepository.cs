using System;
using System.Collections.Generic;
using System.Linq;
using System.Models;
using System.Threading.Tasks;

namespace System_Back_End.Repositories
{
    public interface IComplainsRepository
    {
        IQueryable<Complain> GetAll();
        Task<Complain> GetById(string id);
        Task<bool> Add(Complain complain);
        Task<Complain> Delete(string id);
        Task<bool> Update(Complain complain);
        Task<bool> ComplainExists(string id);
    }
}
