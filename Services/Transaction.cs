using System;
using System.Data.Common;
using Fastdo.Core.Models;
using Fastdo.Core.Services;
using Microsoft.EntityFrameworkCore.Storage;

namespace Fastdo.API.Services
{
    public class TransactionService: ITransactionService
    {
        private SysDbContext _context { get; set; }
        private IDbContextTransaction _actionOnDbTransaction { get; set; }
        public TransactionService(SysDbContext context)
        {
            _context = context;
        }
        public ITransactionService TakeActionOnDb(Action<SysDbContext> option)
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
        public ITransactionService TakeActionOnDb(Action option)
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
        public ITransactionService CommitChanges()
        {
            _actionOnDbTransaction.Commit();
            return this;
        }
        public DbConnection GetConnection()
        {
           return _actionOnDbTransaction.GetDbTransaction().Connection;
        }
        public ITransactionService RollBackChanges()
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
