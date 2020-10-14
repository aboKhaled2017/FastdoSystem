using Fastdo.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Controllers.Stocks
{
    public class HandlePharmaRequestModel
    {
        public bool Seen { get; set; }
        public PharmacyRequestStatus Status { get; set; }
        public string PharmaClass { get; set; }
    }
}
