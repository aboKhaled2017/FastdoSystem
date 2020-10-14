using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fastdo.Core.Models;
using System.Threading.Tasks;
using Fastdo.backendsys.Services;
using System.Text;
using System.Data.Common;
using Fastdo.Core;

namespace Fastdo.backendsys.Repositories
{
    public class Repository<TModel>:IRepository<TModel> where TModel:class
    {
        protected SysDbContext _context { get; }
        public Repository(SysDbContext context)
        {
            _context = context;
        }
        protected string UserId
        {
            get
            {
                return RequestStaticServices.GetUserManager().GetUserId(RequestStaticServices.GetCurrentHttpContext().User);
            }
        }
        protected string UserName
        {
            get
            {
                return RequestStaticServices.GetUserManager().GetUserName(RequestStaticServices.GetCurrentHttpContext().User);
            }
        }
        public bool Save()
        {
            return _context.SaveChanges()>0;
        }
        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> UpdateFieldsAsync_And_Save(
           TModel entity,
            params Expression<Func<TModel, object>>[] updatedProperties)
        {
            EntityEntry<TModel> entityEntry = _context.Entry(entity);
            entityEntry.State = EntityState.Modified;
            if (updatedProperties.Any())
            {
                //update explicitly mentioned properties
                foreach (var property in updatedProperties)
                {
                    entityEntry.Property(property).IsModified = true;
                }
            }
            else
            {
                //no items mentioned, so find out the updated entries
                foreach (var property in entityEntry.OriginalValues.Properties)
                {
                    var original = entityEntry.OriginalValues.GetValue<object>(property);
                    var current = entityEntry.CurrentValues.GetValue<object>(property);
                    if (original != null && !original.Equals(current))
                        entityEntry.Property(updatedProperties.FirstOrDefault(e => e.Type.Equals(property))).IsModified = true;
                }
            }
            return (await _context.SaveChangesAsync()) > 0;
        }

         public void UpdateFields(
            TModel entity, 
            params Expression<Func<TModel, object>>[] updatedProperties
            ) 
        {
            EntityEntry<TModel> entityEntry = _context.Entry(entity);
            entityEntry.State = EntityState.Modified;
            if (updatedProperties.Any())
            {
                //update explicitly mentioned properties
                foreach (var property in updatedProperties)
                {
                    entityEntry.Property(property).IsModified = true;
                }
            }
            else
            {
                //no items mentioned, so find out the updated entries
                foreach (var property in entityEntry.OriginalValues.Properties)
                {
                    var original = entityEntry.OriginalValues.GetValue<object>(property);
                    var current = entityEntry.CurrentValues.GetValue<object>(property);
                    if (original != null && !original.Equals(current))
                        entityEntry.Property(updatedProperties.FirstOrDefault(e => e.Type.Equals(property))).IsModified = true;
                }
            }
        }

         public async Task<TValue> ExecuteScaler<TValue>(StringBuilder query)
        {
            var conn = _context.Database.GetDbConnection();
            if(conn.State==System.Data.ConnectionState.Closed)
            await conn.OpenAsync();
            using (var command = conn.CreateCommand())
            {
                command.CommandText = query.ToString();
                command.CommandType = System.Data.CommandType.Text;
                var res = command.ExecuteScalar();
                return (TValue)res;
            }
        }


        public TModel GetById<T>(T id)
        {
           return _context.Set<TModel>().Find(id);
        }

        public async Task<TModel> GetByIdAsync<T>(T id)
        {
            return await _context.Set<TModel>().FindAsync(id);
        }

        public IQueryable<TModel> GetAll()
        {
            return Where(m => true);
        }
        public IQueryable<TModel> Where(Expression<Func<TModel, bool>> predicate)
        {
            return _context.Set<TModel>().Where(predicate);
        }


        public virtual void Add(TModel model)
        {
            _context.Set<TModel>().Add(model);
        }

        public virtual async Task AddAsync(TModel model)
        {
           await _context.Set<TModel>().AddAsync(model);
        }

        public virtual void AddRange(IEnumerable<TModel> models)
        {
            _context.Set<TModel>().AddRange(models);
        }

        public virtual async Task AddRangeAsync(IEnumerable<TModel> models)
        {
            await _context.Set<TModel>().AddRangeAsync(models);
        }

        public virtual void Remove(TModel model)
        {
            _context.Set<TModel>().Remove(model);
        }

        public virtual void RemoveRange(IEnumerable<TModel> models)
        {
            _context.Set<TModel>().RemoveRange(models);
        }
    }
}
