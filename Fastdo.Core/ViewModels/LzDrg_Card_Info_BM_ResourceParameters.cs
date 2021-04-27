
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.Core
{
    public interface ILzDrg_Card_Info_BM_ResourceParameters : IResourceParameters
    {
         string S { get; set; } 
         IEnumerable<byte> CityIds { get; set; }
         IEnumerable<byte> AreaIds { get; set; }
         string PhramId { get; set; }
         DateTime ValidBefore { get; set; }
         string OrderBy { get; set; }
    }
}
