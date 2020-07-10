using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace DocumentManagementSystem.Attributes
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;
        public string Message => $"Invalid file extension. Allowed extenstion(s) are {string.Join(", ", _extensions)}";
        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            var document = value as IFormFile;
            if (document != null)
            {
                var extension = Path.GetExtension(document.FileName);
                if (!_extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult(Message);
                }
            }
            return ValidationResult.Success;
        }
    }
}
