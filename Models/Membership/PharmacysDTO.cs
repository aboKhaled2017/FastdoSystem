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
        [Required(ErrorMessage = "رقم التليفون الارضى مطلوب")]
        [StringLength(15, MinimumLength = 4, ErrorMessage = "رقم تليفون غير صالح")]
        public string LandlinePhone { get; set; }
        public string Address { get; set; }
    }
    public class Stk_Contacts_Update 
    {
        [Required(ErrorMessage = "رقم التليفون الارضى مطلوب")]
        [StringLength(15, MinimumLength = 4, ErrorMessage = "رقم تليفون غير صالح")]
        public string LandlinePhone { get; set; }
        public string Address { get; set; }
    }
}
