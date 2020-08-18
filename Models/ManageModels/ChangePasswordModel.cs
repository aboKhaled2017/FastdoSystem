using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Fastdo.backendsys.Utilities;

namespace Fastdo.backendsys.Models
{
    [RequireConfirmedEmail]
    public class ChangePasswordModel
    {
        [Required(ErrorMessage ="كلمة السر القديمة مطلوبة")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "كلمة السر مطلوبة")]
        [StringLength(100, ErrorMessage = "كلمة السر على الاقل {1} من الرموز وعلى الاكثر {2} من الرموز", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required(ErrorMessage ="تأكيد كلمة السر مطلوب")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "كلمة السر وتأكيدها غير متطابقتان")]
        public string ConfirmPassword { get; set; }

    }
}
