using Fastdo.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.Core.ViewModels.Stocks
{
    public class HandleStkDrugsRequestModel
    {
        public bool Seen { get; set; }
        public StkDrugPackageRequestStatus Status { get; set; }
    }
}
