using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace System_Back_End.Repositories
{
    public interface IMainRepository
    {
        Task<bool> SaveAsync();
        bool Save();
        Task<bool> UpdateFields<T>(
           T entity,
            params Expression<Func<T, object>>[] updatedProperties)
            where T : class;
    }
}
