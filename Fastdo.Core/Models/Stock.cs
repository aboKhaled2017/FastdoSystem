using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fastdo.Core.Enums;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Fastdo.Core.Models
{
    public partial class Stock
    {
        public Stock()
        {
            SDrugs = new HashSet<StkDrug>();
            Status = StockRequestStatus.Pending;
        }

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
        public StockRequestStatus Status { get; set; }

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
        [InverseProperty("Stock")]
        public virtual ICollection<StockWithPharmaClass> PharmasClasses { get; set; }
        public virtual ICollection<StkDrug> SDrugs { get; set; }
        [InverseProperty("Stock")]
        public virtual ICollection<PharmacyInStock> GoinedPharmacies { get; set; }
        [InverseProperty("Stock")]
        public virtual ICollection<StockInStkDrgPackageReq> RequestedPackages { get; set; }

    }
}
