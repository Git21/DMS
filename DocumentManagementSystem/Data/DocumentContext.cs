using DocumentManagementSystem.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManagementSystem.Data
{
    public class DocumentContext : DbContext
    {
        public DocumentContext() : base()
        {

        }

        public DbSet<Document> Documents { get; set; }

    }
}
