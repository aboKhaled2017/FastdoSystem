using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Controllers.Pharmacies
{
    public class StkDrugsReqOfPharmaModel
    {
        public string StockId { get; set; }
        public IEnumerable<IEnumerable<dynamic>> DrugsList { get; set; }
    }
}
