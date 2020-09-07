using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Models
{
    public class ShowAdminModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string SuperId { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Type { get; set; }
        public string Priviligs { get; set; }
    }
}
