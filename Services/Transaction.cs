using System;
using System.Data.Common;
using Fastdo.Core.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace Fastdo.backendsys.Services
{
    public class TransactionService
    {
        private SysDbContext _context { get; set; }
        private IDbContextTransaction _actionOnDbTransaction { get; set; }
        public TransactionService(SysDbContext context)
        {
            _context = context;
        }
        public TransactionService TakeActionOnDb(Action<SysDbContext> option)
        {
            try
            {
                option.Invoke(_context);
            }
            catch
            {
                _actionOnDbTransaction.Rollback();
            }

            return this;
        }
        public TransactionService TakeActionOnDb(Action option)
        {
            try
            {

                option.Invoke();
            }
            catch
            {
                _actionOnDbTransaction.Rollback();
            }
            return this;
        }
        public TransactionService CommitChanges()
        {
            _actionOnDbTransaction.Commit();
            return this;
        }
        public DbConnection GetConnection()
        {
           return _actionOnDbTransaction.GetDbTransaction().Connection;
        }
        public TransactionService RollBackChanges()
        {
            _actionOnDbTransaction.Rollback();
            return this;
        }
        public void Begin()
        {
            _actionOnDbTransaction = _context.Database.BeginTransaction();
        }
        public void BeginAgain()
        {
            _actionOnDbTransaction = _context.Database.BeginTransaction();
        }
        public void End()
        {
            _actionOnDbTransaction.Dispose();
        }
    }
}
