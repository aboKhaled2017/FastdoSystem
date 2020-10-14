using Fastdo.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Controllers.Pharmacies
{
    public class ShowStkDrugsPackagePhModel
    {
        public ShowStkDrugsPackagePhModel(DateTime createdAt)
        {
            CreatedAt = createdAt.Date.ToShortDateString();
        }
        public Guid PackageId { get; set; }
        public string CreatedAt { get; set; }
        public string Name { get; set; }
        public IEnumerable<ShowStkDrugsPackagePh_FromStockModel> FromStocks { get; set; }
    }

    public class ShowStkDrugsPackagePh_FromStockModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Guid StockClassId { get; set; }
        public StkDrugPackageRequestStatus Status { get; set; }
        public bool Seen { get; set; }
        public string Address { get; set; }
        public string AddressInDetails { get; set; }
        public IEnumerable<ShowStkDrugsPackagePh_FromStock_DrugModel> Drugs { get; set; }
    }
    public class ShowStkDrugsPackagePh_FromStock_DrugModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public dynamic Discount { get; set; }
    }
    
}
