using Fastdo.Repositories.Enums;
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
    }
    public class Show_VStock_LzDrg_ADM_Model
    {       
        public string Name { get; set; }
        public IEnumerable<VStock_LzDrg_For_Pharmacy_ADM_Model> Products { get; set; }

        public IDictionary<LzDrugUnitType,int> TotalQuantity { get {
                var count = new Dictionary<LzDrugUnitType, int>();           
                var groupdData = Products.GroupBy(p => p.UnitType).ToArray();
                for(int i = 0; i < groupdData.Length; i++)
                {
                    int quantity = 0;
                    groupdData[i].ToList().ForEach(drug =>
                    {
                        quantity += drug.Quantity;
                    });
                    count.Add(groupdData[i].First().UnitType, quantity);
                }
                return count;
             
            } 
        }

    }
}
