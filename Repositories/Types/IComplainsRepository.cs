using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Repositories.Models;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Repositories
{
    public interface IComplainsRepository:IMainRepository
    {
        IQueryable<Complain> GetAll();
        Task<Complain> GetById(Guid id);
        Task<bool> Add(Complain complain);
        Task<Complain> Delete(Guid id);
        Task<bool> Update(Complain complain);
        Task<bool> ComplainExists(Guid id);
    }
}
