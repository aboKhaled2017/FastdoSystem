using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System_Back_End.Services;

namespace System_Back_End.Utilities
{
    public class ValidateDrugValidDate : ValidationAttribute
    {
        public ValidateDrugValidDate()
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value==null)
                return new ValidationResult(ErrorMessage ?? "تاريخ الصلاحية مطلوب");
            DateTime dateVal;
            if(!DateTime.TryParse(value.ToString(), out dateVal))
                return new ValidationResult(ErrorMessage ?? "من فضلك ادخل تاريخ صلاحية الراكد");
            if (dateVal==null)
                return new ValidationResult(ErrorMessage ?? "من فضلك ادخل تاريخ صلاحية الراكد");
            var com = dateVal.Date.CompareTo(DateTime.Now.Date);
            if(com<=0)
                return new ValidationResult(ErrorMessage ?? "تاريخ غير صالح");
            return ValidationResult.Success;
        }
    }
}
