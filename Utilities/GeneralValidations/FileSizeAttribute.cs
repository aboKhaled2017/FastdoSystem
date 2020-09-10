using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Utilities
{
    /// <summary>
    /// Validation attribute to assert an <see cref="IFormFile">IFormFile</see> property, field or parameter does not exceed a maximum size.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class FileSizeAttribute : ValidationAttribute
    {
        /// <summary>
        /// Gets the maximum acceptable size of the file.
        /// </summary>
        public long MaximumSize { get; private set; }

        /// <summary>
        /// Gets or sets the minimum acceptable size of the file.
        /// </summary>
        public int MinimumSize { get; set; }

        /// <summary>
        /// Constructor that accepts the maximum size of the file.
        /// </summary>
        /// <param name="maximumSize">The maximum size, inclusive.  It may not be negative.</param>
        public FileSizeAttribute(int maximumSize) : base(() =>"حجم الملف اكبر من المعتاد")
        {
            MaximumSize = maximumSize;
        }

        /// <summary>
        /// Override of <see cref="ValidationAttribute.IsValid(object)"/>
        /// </summary>
        /// <remarks>
        /// This method returns <c>true</c> if the <paramref name="value"/> is null.  
        /// It is assumed the <see cref="RequiredAttribute"/> is used if the value may not be null.
        /// </remarks>
        /// <param name="value">The value to test.</param>
        /// <returns><c>true</c> if the value is null or it's size is less than or equal to the set maximum size</returns>
        /// <exception cref="InvalidOperationException"> is thrown if the current attribute is ill-formed.</exception>
        public override bool IsValid(object value)
        {
            // Check the lengths for legality
            EnsureLegalSizes();

            // Automatically pass if value is null. RequiredAttribute should be used to assert a value is not null.
            // We expect a cast exception if the passed value was not an IFormFile.
            var length = value == null ? 0 : ((IFormFile)value).Length;

            return value == null || (length >= MinimumSize && length <= MaximumSize);
        }

        /// <summary>
        /// Override of <see cref="ValidationAttribute.FormatErrorMessage"/>
        /// </summary>
        /// <param name="name">The name to include in the formatted string</param>
        /// <returns>A localized string to describe the maximum acceptable size</returns>
        /// <exception cref="InvalidOperationException"> is thrown if the current attribute is ill-formed.</exception>
        public override string FormatErrorMessage(string name)
        {
            EnsureLegalSizes();

            string errorMessage = MinimumSize != 0 ? $"حجم الملف يجب  ان يكون على الاقل {MinimumSize} bytes" : ErrorMessageString;

            // it's ok to pass in the minLength even for the error message without a {2} param since String.Format will just ignore extra arguments
            return string.Format(CultureInfo.CurrentCulture, errorMessage, name, MaximumSize, MinimumSize);
        }

        /// <summary>
        /// Checks that MinimumSize and MaximumSize have legal values.  Throws InvalidOperationException if not.
        /// </summary>
        private void EnsureLegalSizes()
        {
            if (MaximumSize < 0)
            {
                throw new InvalidOperationException($"حجك الفايل لا يجب ان يتعدى {MaximumSize} bytes");
            }

            if (MaximumSize < MinimumSize)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,$"minimum size shouldn't be greater than maximum size ", MaximumSize, MinimumSize));
            }
        }
    }
}
