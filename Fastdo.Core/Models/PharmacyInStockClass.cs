using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fastdo.Core.Models
{
    public partial class PharmacyInStockClass
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string PharmacyId { get; set; }
        public Guid StockClassId { get; set; }
        [ForeignKey("PharmacyId")]
        [InverseProperty("StocksClasses")]
        public virtual Pharmacy Pharmacy { get; set; }
        [ForeignKey("StockClassId")]
        [InverseProperty("PharmaciesOfThatClass")]
        public virtual StockWithPharmaClass StockClass { get; set; }
    }
}
