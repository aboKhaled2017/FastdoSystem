using Fastdo.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.Core.ViewModels.Stocks
{
    public class HandlePharmaRequestModel
    {
        public bool Seen { get; set; }
        public PharmacyRequestStatus Status { get; set; }
        public string PharmaClass { get; set; }
    }
}
