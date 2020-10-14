using Fastdo.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Models
{
    public class Get_PageOf_Stocks_ADMModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string MgrName { get; set; }
        public string OwnerName { get; set; }
        public string PersPhone { get; set; }
        public string LandlinePhone { get; set; }
        public string LicenseImgSrc { get; set; }
        public string CommercialRegImgSrc { get; set; }
        public StockRequestStatus Status { get; set; }
        public string Address { get; set; }
        public byte AreaId { get; set; }
        public int joinedPharmesCount { get; set; }
        public int drugsCount { get; set; }

    }
}
