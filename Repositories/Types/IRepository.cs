using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Repositories
{
    public interface IRepository
    {
        Task<bool> SaveAsync();
        bool Save();

        /*TModel Get(int id);
        Task<TModel> GetAsync(int id);

        IEnumerable<TModel> GetAll();
        Task<IEnumerable<TModel>> GetAllAsync();

        IEnumerable<TModel> Find(Expression<Func<TModel, bool>> predicate);
        Task<IEnumerable<TModel>> FindAsync(Expression<Func<TModel, bool>> predicate);

        void Add(TModel model);
        Task AddAsync(TModel model);

        void AddRange(IEnumerable<TModel> models);
        Task AddRangeAsunc(IEnumerable<TModel> models);

        void Remove(TModel model);
        Task RemoveAsync(TModel model);

        void RemoveRange(IEnumerable<TModel> models);
        Task RemoveRangeAsync(IEnumerable<TModel> models);*/
    }
}
