using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManagementSystem.Model
{
    public class Document
    {
        [Key]
        public string DocId { get; set; }
        public int DisplayOrder { get; set; }
        [MaxLength(250)]
        [Required]
        public string Name { get; set; }
        [Required]
        public string Location { get; set; }
        public long FileSize { get; set; }
    }
}
