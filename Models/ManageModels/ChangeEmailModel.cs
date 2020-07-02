using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System_Back_End.Utilities;

namespace System_Back_End.Models
{
    public class ChangeEmailModel
    {
        [EmailAddress(ErrorMessage = "بريد الكترونى غير صالح")]
        [CheckIfUserPropValueIsExixtsOnUpdate("NewEmail",UserPropertyType.email)]
        [Required(ErrorMessage ="البريد الالكترونى الجديد مطلوب")]
        public string NewEmail { get; set; }
    }
}
