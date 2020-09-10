using Fastdo.Repositories.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Controllers.Stocks
{
    public class ShowPharmaReqToStkModelModel
    {
        public string PharmacyId { get; set; }
        public PharmacyRequestStatus Status { get; set; }
        public string PharmaClass { get; set; }
        public bool Seen { get; set; }
    }
}
