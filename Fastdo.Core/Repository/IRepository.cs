using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Fastdo.Core
{
    public interface IRepository<TModel> where TModel: class
    {
        Task<bool> SaveAsync();
        bool Save();

        TModel GetById<TID>(TID id);
        Task<TModel> GetByIdAsync<TID>(TID id);

        IQueryable<TModel> GetAll();

        IQueryable<TModel> Where(Expression<Func<TModel, bool>> predicate);

        void Add(TModel model);
        Task AddAsync(TModel model);

        void AddRange(IEnumerable<TModel> models);
        Task AddRangeAsync(IEnumerable<TModel> models);

        void Remove(TModel model);

        void RemoveRange(IEnumerable<TModel> models);


        Task<bool> UpdateFieldsAsync_And_Save(TModel entity,
            params Expression<Func<TModel, object>>[] updatedProperties);
        void UpdateFields(
            TModel entity,
            params Expression<Func<TModel, object>>[] updatedProperties
            );
        Task<TValue> ExecuteScaler<TValue>(StringBuilder query);
    }
}
