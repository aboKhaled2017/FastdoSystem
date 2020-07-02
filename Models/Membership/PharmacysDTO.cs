using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System_Back_End.Utilities;

namespace System_Back_End.Models
{
    public class Phr_Contacts_Update 
    {
        [CheckIfUserPropValueIsExixtsOnUpdate("PersPhone",UserPropertyType.phone)]
        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [RegularExpression("^((010)|(011)|(012)|(015)|(017))[0-9]{8}$", ErrorMessage = "رقم هاتف غير صالح")]
        public string PersPhone { get; set; }
        [Required(ErrorMessage = "رقم التليفون الارضى مطلوب")]
        [StringLength(15, MinimumLength = 4, ErrorMessage = "رقم تليفون غير صالح")]
        public string LandlinePhone { get; set; }
        public string Address { get; set; }
    }
    public class Stk_Contacts_Update 
    {
        [CheckIfUserPropValueIsExixtsOnUpdate("PersPhone", UserPropertyType.phone)]
        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [RegularExpression("^((010)|(011)|(012)|(015)|(017))[0-9]{8}$", ErrorMessage = "رقم هاتف غير صالح")]
        public string PersPhone { get; set; }
        [Required(ErrorMessage = "رقم التليفون الارضى مطلوب")]
        [StringLength(15, MinimumLength = 4, ErrorMessage = "رقم تليفون غير صالح")]
        public string LandlinePhone { get; set; }
        public string Address { get; set; }
    }
}
