using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Fastdo.backendsys.Utilities;

namespace Fastdo.backendsys.Models
{
    public class UpdateSubAdminPasswordModel
    {
        [Required(ErrorMessage = "كلمة السر مطلوبة")]
        [StringLength(100, ErrorMessage = "كلمة السر على الاقل {1} من الرموز وعلى الاكثر {2} من الرموز", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "تأكيد كلمة السر مطلوب")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "كلمة السر وتأكيدها غير متطابقتان")]
        public string ConfirmPassword {get; set;}
    }
    public class UpdateSubAdminPhoneNumberModel
    {
        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [RegularExpression("^((010)|(011)|(012)|(015)|(017))[0-9]{8}$", ErrorMessage = "رقم هاتف غير صالح")]
        public string PhoneNumber { get; set; }
    }
    public class UpdateSubAdminUserNameModel
    {
        [Required(ErrorMessage = "الاسم المستخد مطلوب")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "اسم المستخدم ما بين {1} الى {2} حرف")]
        [CheckIfUserPropValueIsExixtsOnUpdate("NewUserName", UserPropertyType.userName)]
        public string NewUserName { get; set; }
    }
    public class UpdateSubAdminModel
    {

        [Required(ErrorMessage = "الاسم مطلوب")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "الاسم مابين 5 الى 20 حرف")]
        public string Name { get; set; }

        public string Priviligs { get; set; }
    }
}
