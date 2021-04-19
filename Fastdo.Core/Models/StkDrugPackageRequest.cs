using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fastdo.Core.Enums;
using System.Text;

namespace Fastdo.Core.Models
{
    public class StkDrugPackageRequest
    {
        public StkDrugPackageRequest()
        {
            CreateAt = DateTime.Now;
        }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string PharmacyId { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string PackageDetails { get; set; }
        public DateTime CreateAt { get; set; }

        [ForeignKey("PharmacyId")]
        [InverseProperty("RequestedStkDrugsPackages")]
        public virtual Pharmacy Pharmacy { get; set; }
        [InverseProperty("Package")]
        public virtual ICollection<StockInStkDrgPackageReq> AssignedStocks { get; set; }
    }
}
