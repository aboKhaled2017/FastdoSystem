using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Models;
using System.Threading.Tasks;

namespace System_Back_End.Repositories
{
    public class MainRepository:IMainRepository
    {
        protected SysDbContext _context { get; }
        public MainRepository(SysDbContext context)
        {
            _context = context;
        }
        public bool Save()
        {
            return _context.SaveChanges()>0;
        }
        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> UpdateFields<T>(
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
    }
}
