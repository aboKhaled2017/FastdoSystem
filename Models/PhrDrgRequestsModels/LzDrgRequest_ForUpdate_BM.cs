using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Enums;

namespace System_Back_End.Models
{
    public class LzDrgRequest_ForUpdate_BM
    {
        public bool Seen { get; set; }
        public PharmacyRequestStatus Status { get; set; }
    }
}
