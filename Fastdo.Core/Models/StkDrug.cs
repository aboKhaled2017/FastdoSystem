using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.Core.Models
{
    //for Stock drug table
    public partial class StkDrug
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set;}
        [Required]
        [StringLength(60,MinimumLength =3)]
        public string Name { get; set; }
        [Required]
        [Range(1,double.MaxValue,ErrorMessage ="من فضلك تأكد من ادخال حقول الاسعار بشكل صحيح ربما يكون هناك حقل فارغ")]
        public double Price { get; set; }
        [Required]
        [Range(0.0, 100.0)]
        public string Discount { get;set;}
        public DateTime LastUpdate {get;} = DateTime.Now;
        [Required]
        public string StockId { get; set; }
        public virtual Stock Stock { get; set; }
        [InverseProperty("StkDrug")]
        public virtual ICollection<StkDrugInStkDrgPackageReq> RequestedPackages { get; set; }

    }
}
