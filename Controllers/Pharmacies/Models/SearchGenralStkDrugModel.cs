using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Controllers.Pharmacies
{
    public class SearchedStkDrugModelOfAllStocks
    {
        public Guid Id { get; set; }
        public virtual string Discount { get; set; }
        public double Price { get; set; }
        public string StockId { get; set; }
        public string StockName { get; set; }
        public bool IsJoinedTo { get; set; }
    }
    public class SearchedStkDrugModelOfAllStocks_TargetPharma: SearchedStkDrugModelOfAllStocks
    {
        public new double Discount { get; set; }
    }
    public class SearchGenralStkDrugModel
    {     
        public string Name { get; set; }
        public virtual IEnumerable<SearchedStkDrugModelOfAllStocks> Stocks { get; set; }
        public int StockCount { get; set; }
    }
    public class SearchGenralStkDrugModel_TargetPharma: SearchGenralStkDrugModel
    {
        public new IEnumerable<SearchedStkDrugModelOfAllStocks_TargetPharma> Stocks { get; set; }
    }
}
