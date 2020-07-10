using DocumentManagementSystem.Interfaces;
using DocumentManagementSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DocumentManagementSystem.Data
{
    public class Repository : IRepository
    {
        private readonly DocumentContext _context;
        public Repository(DocumentContext context)
        {
            _context = context;
        }
        public ICollection<Document> GetDocs()
        {
            return _context.Documents.ToList();
        }
        public Document GetDoc(string Id)
        {
            return _context.Documents.FirstOrDefault(d => d.DocId == Id);
        }

        public void AddDoc(Document doc)
        {
            if (doc == null)
            {
                throw new ArgumentNullException(nameof(doc));
            }
            _context.Documents.Add(doc);
        }

        public void UpdateDoc(Document doc)
        {
            //Do nothing
        }

        public void DeleteDoc(Document doc)
        {
            if (doc == null)
                throw new ArgumentNullException(nameof(doc));

            _context.Documents.Remove(doc);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
