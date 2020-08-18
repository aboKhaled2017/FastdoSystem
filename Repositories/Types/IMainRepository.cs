using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Repositories
{
    public interface IMainRepository
    {
        Task<bool> SaveAsync();
        bool Save();
    }
}
