using System;
using System.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace System_Back_End.Services
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
        public void CommitChanges()
        {
            _actionOnDbTransaction.Commit();
        }
        public void RollBackChanges()
        {
            _actionOnDbTransaction.Rollback();
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
