using System.ComponentModel.DataAnnotations;
using Fastdo.backendsys.Utilities;

namespace Fastdo.backendsys.Models
{
    public interface IPhr_RegisterModel_Account
    {
        [Required(ErrorMessage = "البريد الالكترونى مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الالكترونى غير صحيح")]
        [CheckIfUserPropValueIsExixts("Email", UserPropertyType.email)]
        string Email { get; set; }

        [Required(ErrorMessage = "كلمة السر مطلوبة")]
        [StringLength(100, ErrorMessage = "كلمة السر على الاقل {1} من الرموز وعلى الاكثر {2} من الرموز", MinimumLength = 6)]
        [DataType(DataType.Password)]
        string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "كلمة السر وتأكيدها غير متطابقتان")]
        string ConfirmPassword { get; set; }
    }
}
