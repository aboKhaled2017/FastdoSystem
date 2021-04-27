using Fastdo.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Fastdo.Core.Services
{
    public interface ITransactionService
    {
        ITransactionService TakeActionOnDb(Action<SysDbContext> option);
        ITransactionService TakeActionOnDb(Action option);
        ITransactionService CommitChanges();
        DbConnection GetConnection();
        ITransactionService RollBackChanges();
        void Begin();
        void BeginAgain();
        void End();
    }
}
