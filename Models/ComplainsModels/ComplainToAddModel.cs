using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Models
{
    public class ComplainToAddModel
    {
        [MaxLength(50,ErrorMessage ="الاسم لايزيد عن 50 حرف")]
        public string Name { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [EmailAddress(ErrorMessage ="البريد الالكترونى غير صحيح")]
        public string Email { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [MaxLength(100, ErrorMessage = "الموضوع لايزيد عن 100 حرف")]
        public string Subject { get; set; }
        [Required(ErrorMessage ="هذا الحقل مطلوب")]
        public string Message { get; set; }
    }
}
