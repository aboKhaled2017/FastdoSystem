using Fastdo.Core.Enums;
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
        public bool Seen { get; set; }
        public string Address { get; set; }
        public string AddressInDetails { get; set; }
        public string Name { get; set; }
        public string PersPhone { get; set; }
        public string LandLinePhone { get; set; }
    }
  
}
