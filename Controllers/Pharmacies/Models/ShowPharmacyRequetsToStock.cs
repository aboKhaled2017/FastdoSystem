using Fastdo.Repositories.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Controllers.Pharmacies
{
    public class ShowSentRequetsToStockByPharmacyModel
    {
        public string StockId { get; set; }
        public PharmacyRequestStatus Status { get; set; }
        public string PharmaClass { get; set; }
        public bool Seen { get; set; }
    }
  
}
