using Fastdo.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Controllers.Stocks
{
    public class HandleStkDrugsRequestModel
    {
        public bool Seen { get; set; }
        public StkDrugPackageRequestStatus Status { get; set; }
    }
}
