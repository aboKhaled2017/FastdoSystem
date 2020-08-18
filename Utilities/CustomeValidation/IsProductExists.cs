using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Fastdo.backendsys.Services;

namespace Fastdo.backendsys.Utilities
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IsProductExists:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var productId = Guid.Empty;
            if (Guid.TryParse(value.ToString(), out productId) &&
               RequestStaticServices.GetDbContext().LzDrugs.Any(p => p.Id == productId))
                return ValidationResult.Success;
            return new ValidationResult(string.Format("the product id ${0} was not found",value));
        }
    }
}
