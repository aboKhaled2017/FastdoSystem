using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Utilities
{
    public class AllowedExtensions : ValidationAttribute
    {
        private readonly string[] _extensions;
        public AllowedExtensions(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            if (value == null) return null;
            var file = value as IFormFile;
            var extension = Path.GetExtension(file.FileName).Replace(".","");
            if (file != null)
            {
                if (!_extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"امتداد هذا الفايل غير مدعوم";
        }
    }
}
