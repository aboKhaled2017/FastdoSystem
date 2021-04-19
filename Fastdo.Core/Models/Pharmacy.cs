using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fastdo.Core.Enums;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace Fastdo.Core.Models
{
    public partial class Pharmacy
    {
        public Pharmacy() {
            this.LzDrugs = new HashSet<LzDrug> ();
            this.Status = PharmacyRequestStatus.Pending;
            this.RequestedLzDrugs = new HashSet<LzDrugRequest>();
            StocksClasses = new Collection<PharmacyInStockClass>();
        }
        public PharmacyRequestStatus Status { get; set; }
        [Key]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string MgrName { get; set; }

        [Required]
        public string OwnerName { get; set; }

        [Required]
        public string LicenseImgSrc { get; set; }

        [Required]
        public string CommercialRegImgSrc { get; set; }

        [Required]
        [Phone]
        public string PersPhone { get; set; }

        [Required]
        [Phone]
        public string LandlinePhone { get; set; }

        public string Address { get; set; }

        [Required]
        public byte AreaId { get; set; }
        public virtual Area Area { get; set; }
      
        [ForeignKey("Id")]
        public virtual AppUser User { get; set; }
        public virtual ICollection<LzDrug> LzDrugs { get; set; }
        [InverseProperty("Pharmacy")]
        public virtual ICollection<PharmacyInStock> GoinedStocks { get; set; }
        //are requests the pharmacy made
        [InverseProperty("Pharmacy")]
        public virtual ICollection<LzDrugRequest> RequestedLzDrugs { get; set; }

        [InverseProperty("Pharmacy")]
        public virtual ICollection<StkDrugPackageRequest> RequestedStkDrugsPackages { get; set; }

        [InverseProperty("Pharmacy")]
        public virtual ICollection<PharmacyInStockClass> StocksClasses { get; set; }
    }
}