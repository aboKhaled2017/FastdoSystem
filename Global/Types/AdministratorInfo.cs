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
    public static class AdminPreviligs
    {
        public static string FullControlOnSubAdmins { get; } = "FullControlOnSubAdmins";
        public static string ViewAnySubAdmin { get; } = "ViewAnySubAdmin";
        public static string AddNewAdmin { get; } = "AddNewAdmin";
        public static string UpdateSubAdmin { get; } = "UpdateSubAdmin";
        public static string DeleteSubAdmin { get; } = "DeleteSubAdmin";
        
        public static string AddNewRepresentative { get;} = "AddNewRepresentative";
        public static string UpdateRepresentative { get; } = "UpdateRepresentative";
        public static string DeleteRepresentative { get; } = "DeleteRepresentative";

        public static string AddNewSuperVisor { get;} = "AddNewSuperVisor";
        public static string UpdateSuperVisor { get; } = "UpdateSuperVisor";
        public static string DeleteSuperVisor { get; } = "DeleteSuperVisor";

    }

}
