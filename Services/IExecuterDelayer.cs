using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Services
{
    public interface IExecuterDelayer
    {
         Action OnExecuting { get; set; }
        void Execute();
        
    }
}
