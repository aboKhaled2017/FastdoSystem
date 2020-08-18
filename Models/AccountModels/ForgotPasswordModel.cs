using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Fastdo.backendsys.Utilities;

namespace Fastdo.backendsys.Models
{
    public class ForgotPasswordModel
    {
        [Required(ErrorMessage ="البريد الالكترونى مطلوب")]
        [EmailAddress(ErrorMessage ="البريد الالكترونى غير صالح")]
        [RequireConfirmedEmail(checkForEmailIfFound:true)]
        public string Email { get; set; }
    }
    public class ForgotAndResetPasswordModel
    {
        [Required(ErrorMessage = "البريد الالكترونى مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الالكترونى غير صالح")]
        public string Email { get; set; }
        [Required(ErrorMessage = "كلمة السر مطلوبة")]
        [StringLength(100, ErrorMessage = "كلمة السر على الاقل {1} من الرموز وعلى الاكثر {2} من الرموز", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "كلمة السر وتأكيدها غير متطابقتان")]
        public string ConfirmNewPassword { get; set; }
    }
}
