using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fastdo.Core.Enums;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.Core.Models
{
    public partial class PharmacyInStock
    {
        public PharmacyInStock()
        {
            //Pharmacy = new Pharmacy();
            //Stock = new Stock();
            PharmacyReqStatus = 0;
            Seen = false;
        }
        [Key, Column(Order = 0)]
        public string PharmacyId { get; set; }
        [Key, Column(Order = 1)]
        public string StockId { get; set; }
        [ForeignKey("PharmacyId")]
        [InverseProperty("GoinedStocks")]
        public virtual Pharmacy Pharmacy  { get; set; }
        [ForeignKey("StockId")]
        [InverseProperty("GoinedPharmacies")]
        public virtual Stock Stock { get; set; }
        public PharmacyRequestStatus PharmacyReqStatus { get; set; }
        public bool Seen { get; set; }
    }
    
}
