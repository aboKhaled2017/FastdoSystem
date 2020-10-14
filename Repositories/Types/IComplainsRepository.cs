using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Core.Models;
using System.Threading.Tasks;
using Fastdo.Core;

namespace Fastdo.backendsys.Repositories
{
    public interface IComplainsRepository:IRepository<Complain>
    {
        Task<bool> ComplainExists(Guid id);
        Task Update(Complain complain);
        Task<Complain> Delete(Guid id);


    }
}
