using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Controllers.Stocks.Models
{
    public class StkShowDrugModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Discount { get; set; }
        public double Price { get; set; }
    }
}
