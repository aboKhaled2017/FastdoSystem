using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Fastdo.Core.Enums;
using Fastdo.Core.Utilities;

namespace Fastdo.Core.ViewModels
{
    public class ChangeEmailModel
    {
        [EmailAddress(ErrorMessage = "بريد الكترونى غير صالح")]
        [CheckIfUserPropValueIsExixtsOnUpdate("NewEmail",UserPropertyType.email)]
        [Required(ErrorMessage ="البريد الالكترونى الجديد مطلوب")]
        public string NewEmail { get; set; }
    }
}
