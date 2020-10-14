using System;
using Fastdo.Core.Enums;

namespace Fastdo.backendsys.Models
{
    public class Made_LzDrgRequest_MB
    {
        public Guid Id { get; set; }
        public Guid LzDrugId { get; set; }
        public string LzDrugName{ get; set; }
        public LzDrugRequestStatus Status { get; set; }
        public string PharmacyId { get; set; }
        public string PhName { get; set; }
    }
}
