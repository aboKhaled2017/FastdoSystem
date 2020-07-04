using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System_Back_End
{
    public abstract class  ResourceParameters
    {
        virtual public int PageNumber { get; set; } 
        virtual public int PageSize
        {
            get;set;
        }
    }
}
