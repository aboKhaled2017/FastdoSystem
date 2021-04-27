
using System.ComponentModel.DataAnnotations;
using Fastdo.Core.Enums;
using Fastdo.Core.Utilities;

namespace Fastdo.Core.ViewModels
{
    public class Phr_RegisterModel_Account 
    {
        [Required(ErrorMessage = "البريد الالكترونى مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الالكترونى غير صحيح")]
        [CheckIfUserPropValueIsExixts("Email", UserPropertyType.email)]
        public string Email { get; set; }

        [Required(ErrorMessage = "كلمة السر مطلوبة")]
        [StringLength(100, ErrorMessage = "كلمة السر على الاقل {1} من الرموز وعلى الاكثر {2} من الرموز", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "كلمة السر وتأكيدها غير متطابقتان")]
        public string ConfirmPassword { get; set; }
    }
}
