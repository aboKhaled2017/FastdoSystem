using System;
using System.Collections.Generic;
using System.Enums;
using System.Linq;
using System.Threading.Tasks;

namespace System_Back_End.Models
{
    public class ShowLzDrugModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }       
        public string Type { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public LzDrugConsumeType ConsumeType { get; set; } 
        public double Discount { get; set; }
        public DateTime ValideDate { get; set; }
        public LzDrugPriceState PriceType { get; set; } 
        public LzDrugUnitType UnitType { get; set; }
        public string Desc { get; set; }
        public int RequestCount { get; set; }
    }
}
