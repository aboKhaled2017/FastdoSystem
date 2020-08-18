using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Services
{
    public class ExecuterDelayer:IExecuterDelayer
    {
        public Action OnExecuting { get; set; }
        public void Execute()
        {
            this.OnExecuting.Invoke();
        }
    }
}
