using Fastdo.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Models
{
    public class Get_PageOf_Pharmacies_ADMModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string MgrName { get; set; }
        public string OwnerName { get; set; }
        public string PersPhone { get; set; }
        public string LandlinePhone { get; set; }
        public string LicenseImgSrc { get; set; }
        public string CommercialRegImgSrc { get; set; }
        public PharmacyRequestStatus Status { get; set; }
        public string Address { get; set; }
        public byte AreaId { get; set; }
        public int joinedStocksCount { get; set; }
        public int lzDrugsCount { get; set; }
        public int requestedDrugsCount { get; set; }

    }
}
