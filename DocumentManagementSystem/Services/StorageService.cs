using DocumentManagementSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManagementSystem.Services
{
    public class StorageService : IStorageService
    {
        public bool DeleteFile(string fileId)
        {
            throw new NotImplementedException();
        }

        public byte[] DownloadFile(string fileId)
        {
            throw new NotImplementedException();
        }

        public string GetFilePath(string fileId)
        {
            throw new NotImplementedException();
        }

        public string UploadFile(Stream fileStream)
        {
            throw new NotImplementedException();
        }
    }
}
