using DocumentManagementSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManagementSystem.Interfaces
{
    public interface IRepository
    {
        ICollection<Document> GetDocs();
        Document GetDoc(string Id);
        void AddDoc(Document doc);
        void UpdateDoc(Document doc);
        void DeleteDoc(Document doc);
        void Save();

    }
}
