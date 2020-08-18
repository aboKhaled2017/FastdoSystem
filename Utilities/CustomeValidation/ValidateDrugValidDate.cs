using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Fastdo.backendsys.Services;

namespace Fastdo.backendsys.Utilities
{
    public class ValidateDrugValidDate : ValidationAttribute
    {
        public ValidateDrugValidDate()
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string val = (value==null)? string.Empty :value.ToString().Trim();
            if(val ==string.Empty)
                return new ValidationResult(ErrorMessage ?? "تاريخ الصلاحية مطلوب");
            DateTime dateVal;
            if(!DateTime.TryParse(val.ToString(), out dateVal))
                return new ValidationResult(ErrorMessage ?? "من فضلك ادخل تاريخ صلاحية الراكد");
            if(dateVal==default(DateTime))
                return new ValidationResult(ErrorMessage ?? "تاريخ الصلاحية مطلوب");
            if (dateVal==null)
                return new ValidationResult(ErrorMessage ?? "من فضلك ادخل تاريخ صلاحية الراكد");
            var com = dateVal.Date.CompareTo(DateTime.Now.Date);
            if(com<=0)
                return new ValidationResult(ErrorMessage ?? "تاريخ غير صالح");
            return ValidationResult.Success;
        }
    }
}
