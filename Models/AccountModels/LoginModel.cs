using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage ="البريد الالكترونى مطلوب")]
        [EmailAddress(ErrorMessage ="بريد الالكترونى غير صالح")]
        public string Email { get; set; }

        [Required(ErrorMessage ="كلمة السر مطلوبة")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public UserType UserType { get; set; } = UserType.pharmacier;
        public bool RememberMe { get; set; } = true;
    }
}
