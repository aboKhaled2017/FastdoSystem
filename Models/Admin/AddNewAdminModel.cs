using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Fastdo.backendsys.Utilities;

namespace Fastdo.backendsys.Models
{ 
    public class AddNewSubAdminModel
    {
        AddNewSubAdminModel()
        {
            AdminType = Global.AdminType.Administrator;
        }
        [Required(ErrorMessage = "الاسم مطلوب")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "الاسم مابين 5 الى 20 حرف")]
        public string Name { get; set; }
        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [RegularExpression("^((010)|(011)|(012)|(015)|(017))[0-9]{8}$", ErrorMessage = "رقم هاتف غير صالح")]
        [CheckIfUserPropValueIsExixts("PhoneNumber", UserPropertyType.phone)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "الاسم المستخد مطلوب")]
        [StringLength(60,MinimumLength =3,ErrorMessage ="اسم المستخدم ما بين {1} الى {2} حرف")]
        [CheckIfUserPropValueIsExixts("UserName", UserPropertyType.userName)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "كلمة السر مطلوبة")]
        [StringLength(100, ErrorMessage = "كلمة السر على الاقل {1} من الرموز وعلى الاكثر {2} من الرموز", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "كلمة السر وتأكيدها غير متطابقتان")]
        public string ConfirmPassword { get; set; }
        [ValidateAdminType]
        public string AdminType { get; set; }
        [Required(ErrorMessage ="من فضلك اختر على الاقل صلاحية")]
        public string Priviligs { get; set; }
    }
}
