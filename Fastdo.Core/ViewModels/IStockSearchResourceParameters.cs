using System;
using System.Collections.Generic;
using System.Text;

namespace Fastdo.Core.ViewModels
{
    public interface IStockSearchResourceParameters:IResourceParameters
    {

         string S { get; set; }
         IEnumerable<byte> CityIds { get; set; }
         IEnumerable<byte> AreaIds { get; set; }
    }
}
