using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.Core
{
    public interface IResourceParameters
    {
        int PageNumber { get; set; }
        int PageSize
        {
            get; set;
        }
    }
}
