using Fastdo.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Models
{
    public class GetPageOfSearchedStocks
    {
        public string Id { get; set; }
        public string Name { get; set; }
        
        public string PersPhone { get; set; }
        public string LandlinePhone { get; set; }
        public string Address { get; set; }
        public string AddressInDetails { get; set; }

        public byte AreaId { get; set; }
        public int joinedPharmesCount { get; set; }
        public int drugsCount { get; set; }

    }
}
