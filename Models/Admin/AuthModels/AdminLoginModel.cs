using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Fastdo.Core.Enums;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Models
{
    public class AdminLoginModel
    {
        [Required(ErrorMessage = "اسم المستخدم مطلوب")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "كلمة السر مطلوبة")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage ="اختر نوع المسؤل")]
        public string AdminType { get; set; }
        public bool RememberMe { get; set; } = true;
    }
}
