using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.Core.Services
{
    public interface IExecuterDelayer
    {
         Action OnExecuting { get; set; }
        void Execute();
        
    }
}
