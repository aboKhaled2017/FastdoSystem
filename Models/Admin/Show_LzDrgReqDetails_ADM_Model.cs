using Fastdo.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Models
{
    public class Show_LzDrgReqDetails_ADM_Model
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public DateTime ValideDate { get; set; }
        public LzDrugPriceState PriceType { get; set; }
        public LzDrugUnitType UnitType { get; set; }
        public string Desc { get; set; }
        public int RequestCount { get; set; }
    }
}
