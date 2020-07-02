using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System_Back_End.Services
{
    public interface IExecuterDelayer
    {
         Action OnExecuting { get; set; }
        void Execute();
        
    }
}
