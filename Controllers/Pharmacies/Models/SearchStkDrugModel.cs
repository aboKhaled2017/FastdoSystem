using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Controllers.Pharmacies
{
    public class SearchStkDrugModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual string Discount { get; set; }
        public double Price { get; set; }
        public bool JoinedTo { get; set; }
    }
    public class SearchStkDrugModel_TargetPharma: SearchStkDrugModel
    {
        public new double Discount { get; set; }
    }
}
