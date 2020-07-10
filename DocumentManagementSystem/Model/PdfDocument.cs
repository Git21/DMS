using DocumentManagementSystem.Attributes;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DocumentManagementSystem.Model
{
    public class PdfDocument
    {
        [Required(ErrorMessage = "Please select a pdf file to upload.")]
        [DataType(DataType.Upload)]
        [MaxFileSize(5 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".pdf" })]
        public IFormFile Doc { get; set; }
    }
}
