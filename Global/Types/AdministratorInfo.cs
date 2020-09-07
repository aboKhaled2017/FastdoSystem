using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Global
{
    public class AdministratorInfo
    {
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
    public static class AdminType
    {
        public static string Administrator { get; } = "Administrator";
        public static string Representative { get;} = "Representative";
        public static string SuperVisor { get;} = "SuperVisor";
    }

}
