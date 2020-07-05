using System;
using System.Collections.Generic;
using System.Enums;
using System.Linq;
using System.Threading.Tasks;

namespace System_Back_End.Models
{
    public class LzDrugCard_Info_BM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public string ValideDate { get; set; }
        public LzDrugPriceState PriceType { get; set; }
        public LzDrugUnitType UnitType { get; set; }
        public string Desc { get; set; }
        public string PharmacyId { get; set; }

        public string PharmName { get; set; }
        public string PharmLocation { get; set; }

        public string RequestUrl { get; set; }
        public int RequestsCount { get; set; }
        public bool IsMadeRequest { get; set; }
        public LzDrugRequestStatus? Status { get; set; }
    }
}
