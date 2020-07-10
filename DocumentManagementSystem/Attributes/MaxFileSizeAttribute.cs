using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DocumentManagementSystem.Attributes
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;
        public string Message => $"File is too large, max file size is { _maxFileSize} bytes.";
        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;

            if (file?.Length > _maxFileSize)
                return new ValidationResult(Message);

            return ValidationResult.Success;
        }
    }
}
