using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManagementSystem.DTOs
{
    public class DocumentDto
    {
        public string DocId { get; set; }
        public int DisplayOrder { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public long FileSize { get; set; }
    }
}
