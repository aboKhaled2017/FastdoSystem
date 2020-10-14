using Fastdo.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Models
{
    public class Show_LzDrgsReq_ADM_Model
    {
        public Guid Id { get; set; }
        public Guid LzDrugId { get; set; }
        public string RequesterPhram_Id { get; set; }
        public string RequesterPhram_Name { get; set; }
        public string OwenerPh_Id { get; set; }
        public string OwenerPh_Name { get; set; }
        public LzDrugRequestStatus Status { get; set; }
        public string LzDrugName { get; set; }      
    }
}
