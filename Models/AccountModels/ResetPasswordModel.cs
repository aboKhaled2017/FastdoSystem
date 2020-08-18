using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Models
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage ="البريد الالكترونى مطلوب")]
        [EmailAddress(ErrorMessage ="بريد الكترونى غير صحيح")]
        public string Email { get; set; }

        [Required(ErrorMessage ="كلمة السر الجديدة مطلوبة")]
        [StringLength(100, ErrorMessage = "كلمة السر على الاقل 6 حروف", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required(ErrorMessage ="الكود مطلوب")]
        public string Code { get; set; }
    }
}
