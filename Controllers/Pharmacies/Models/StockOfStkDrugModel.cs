using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Controllers.Pharmacies
{
    public class StockOfStkDrugModel
    {
        public virtual string Discount { get; set; }
        public double Price { get; set; }
        public string StockId { get; set; }
        public string StockName { get; set; }
        public bool JoinedTo { get; set; }
    }
    public class StockOfStkDrugModel_TragetPharma:StockOfStkDrugModel
    {
        public new double Discount { get; set; }
    }
}
