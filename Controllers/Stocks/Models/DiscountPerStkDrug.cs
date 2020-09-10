using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Controllers.Stocks.Models
{
    public class DiscountPerStkDrug
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DiscountStr { get; set; }
    }
}
