﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace System_Back_End.Models
{
    public class ConfirmEmailModel
    {
        [Required(ErrorMessage ="البريد الالكترونى مطلوب")]
        [EmailAddress(ErrorMessage = "بريد الالكترونى غير صحيح")]
        public string Email { get; set; }
        [Required(ErrorMessage = "الكود مطلوب")]
        [RegularExpression("^[0-9]{15}$",ErrorMessage ="كود غير صحيح")]
        public string Code { get; set; }
    }
}