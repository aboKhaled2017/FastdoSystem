using Fastdo.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Models
{
    public class VStock_LzDrg_For_Pharmacy_ADM_Model
    {
        public Guid DrugId { get; set; }
        public string PharmacyId { get; set; }
        public string PharmacyName { get; set; }
        public string Type { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public LzDrugConsumeType ConsumeType { get; set; }
        public double Discount { get; set; }
        public DateTime ValideDate { get; set; }
        public LzDrugPriceState PriceType { get; set; }
        public LzDrugUnitType UnitType { get; set; }
        public string Desc { get; set; }
    }
    public class Show_VStock_LzDrg_ADM_Model
    {       
        public string Name { get; set; }
        public string Type { get; set; }
        public IEnumerable<VStock_LzDrg_For_Pharmacy_ADM_Model> Products { get; set; }

    }
}
