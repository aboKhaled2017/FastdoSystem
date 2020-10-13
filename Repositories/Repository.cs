using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fastdo.Repositories.Models;
using System.Threading.Tasks;
using Fastdo.backendsys.Services;
using System.Text;
using System.Data.Common;

namespace Fastdo.backendsys.Repositories
{
    public class Repository
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
        protected async Task<bool> UpdateFieldsAsync_And_Save<T>(
           T entity,
            params Expression<Func<T, object>>[] updatedProperties)
            where T : class
        {
            EntityEntry<T> entityEntry = _context.Entry(entity);
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

        protected void UpdateFields<T>(
            T entity, 
            params Expression<Func<T, object>>[] updatedProperties
            ) where T : class
        {
            EntityEntry<T> entityEntry = _context.Entry(entity);
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

        protected async Task<T> ExecuteScaler<T>(StringBuilder query)
        {
            var conn = _context.Database.GetDbConnection();
            if(conn.State==System.Data.ConnectionState.Closed)
            await conn.OpenAsync();
            using (var command = conn.CreateCommand())
            {
                command.CommandText = query.ToString();
                command.CommandType = System.Data.CommandType.Text;
                var res = command.ExecuteScalar();
                return (T)res;
            }
        }
    }
}
