using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.Core
{
    public abstract class ResourceParameters: IResourceParameters
    {
        virtual public int PageNumber { get; set; }
        virtual public int PageSize
        {
            get; set;
        }
    }
}
