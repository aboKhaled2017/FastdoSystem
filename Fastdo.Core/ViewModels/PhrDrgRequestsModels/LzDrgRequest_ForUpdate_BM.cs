using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fastdo.Core.Enums;

namespace Fastdo.Core.ViewModels
{
    public class LzDrgRequest_ForUpdate_BM
    {
        public bool Seen { get; set; }
        public LzDrugRequestStatus Status { get; set; }
    }
}
