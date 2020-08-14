﻿using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System_Back_End.Global;
using System_Back_End.Services;

namespace System_Back_End.Utilities
{
    public class ValidateAdminType : ValidationAttribute
    {
        public ValidateAdminType()
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string val = (value==null)? string.Empty :value.ToString().Trim();
            if(val ==string.Empty)
                return new ValidationResult(ErrorMessage ?? "اختر نوع المسؤل");
            if(val==AdminType.Administrator ||val==AdminType.Representative||val==AdminType.SuperVisor)
                return ValidationResult.Success;
            return new ValidationResult(ErrorMessage ?? "نوع المسؤل غير صحيح");
        }
    }
}